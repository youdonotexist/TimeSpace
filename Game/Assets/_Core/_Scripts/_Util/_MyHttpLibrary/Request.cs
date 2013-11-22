using UnityEngine;
using System.Collections;
using System.Text;
using System.Net;
using System.IO;
using System;
using MiniJSON;

namespace CI
{
	public class RequestState
	{
		// This class stores the State of the request.
		const int BUFFER_SIZE = 1024;
		public byte[] BufferRead;
		public HttpWebRequest request;
		public HttpWebResponse response;
		public Stream streamResponse;
		public bool isDone = false;
		public bool isCachedDataDone = false;
		
		public string requestData;
		public byte[] requestDataBytes;
		public StringBuilder responseData;
		public Action<CI.Request> callback;
		public CI.Request owningRequest;
		public bool useCache;
		
		public RequestState()
		{
			BufferRead = new byte[BUFFER_SIZE];
			responseData = new StringBuilder("");
			request = null;
			streamResponse = null;
			callback = null; 
			useCache = false;
		}
	}
	
	public class Request
	{
		const int BUFFER_SIZE = 1024;
		public RequestState requestState;
		
		public string ResponseText {
			get { return requestState.responseData.ToString(); }
		}
		
		public Request Send(string url, string action, bool useCache) {
			return Send (url, action, useCache, null);	
		}
		
		public Request Send(string url, string action, Hashtable body) {
			if (string.Compare(action, "put", StringComparison.CurrentCultureIgnoreCase) == 0) {
				return PUT (url, body);	
			}
			else {
				return POST (url, body);	
			}
		}
		
		public Request Send(string url, string action, bool useCache, Action<CI.Request> callback) {
			Debug.Log (url);
			return GET (url, useCache, callback);	
		}
		
		public Request GET(string url, bool useCache, Action<CI.Request> callback) {
			if (useCache) {
				string cached = Cache.Load(WWW.EscapeURL(url));
				if (cached != null) {
					requestState = new RequestState();  
					requestState.responseData.Append(cached);
					requestState.isCachedDataDone = true;
					requestState.owningRequest = this;
					
					if (callback != null) {
						callback(requestState.owningRequest);
					}
				}
			}
			
			if (ResponseCallbackDispatcher.Singleton == null )
            {
                ResponseCallbackDispatcher.Init();
            }
			
			HttpWebRequest request = (HttpWebRequest) HttpWebRequest.Create(new Uri(url));
			request.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1)";
			request.ContentType = "application/json";
			request.Accept = "application/json";
			
			// Create an instance of the RequestState and assign the previous myHttpWebRequest1
			// object to it's request field.  
			requestState = new RequestState();  
			requestState.request = request;
			requestState.callback = callback;
			requestState.owningRequest = this;
			requestState.useCache = useCache;
			
			// Start the asynchronous request.
			IAsyncResult result=
			(IAsyncResult) request.BeginGetResponse(new AsyncCallback(RespCallback),requestState);
			
			return this;
		}
		
		public Request PUT(String url, Hashtable body) {
			HttpWebRequest request = (HttpWebRequest) HttpWebRequest.Create(new Uri(url));
			string requestBodyString = JSON.JsonEncode(body);
			byte[] requestBytes = System.Text.Encoding.ASCII.GetBytes(requestBodyString);
			request.Method = "PUT";
			request.ContentType = "application/json";
     		request.ContentLength = requestBytes.Length;
			
			// Create an instance of the RequestState and assign the previous myHttpWebRequest1
			// object to it's request field.  
			requestState = new RequestState();  
			requestState.request = request;
			requestState.requestData = requestBodyString;
			requestState.requestDataBytes = Encoding.UTF8.GetBytes(requestBodyString);
			requestState.owningRequest = this;
			
			IAsyncResult result = (IAsyncResult) request.BeginGetRequestStream(new AsyncCallback(WriteBodyCallback), requestState);
			return this;
		}
		
		public Request POST(String url, Hashtable body) {
			HttpWebRequest request = (HttpWebRequest) HttpWebRequest.Create(new Uri(url));
			string requestBodyString = Json.Serialize(body);
			byte[] requestBytes = System.Text.Encoding.ASCII.GetBytes(requestBodyString);
			request.Method = "POST";
			request.ContentType = "application/json";
     		request.ContentLength = requestBytes.Length;
			
			// Create an instance of the RequestState and assign the previous myHttpWebRequest1
			// object to it's request field.  
			requestState = new RequestState();  
			requestState.request = request;
			requestState.requestData = requestBodyString;
			requestState.requestDataBytes = Encoding.UTF8.GetBytes(requestBodyString);
			requestState.owningRequest = this;
			
			IAsyncResult result = (IAsyncResult) request.BeginGetRequestStream(new AsyncCallback(WriteBodyCallback), requestState);
			return this;
		}
		
		private static void WriteBodyCallback (IAsyncResult asynchronousRequest) {
			RequestState myRequestState	= (RequestState) asynchronousRequest.AsyncState;
			HttpWebRequest request		= myRequestState.request;

			// End the operation
			Stream postStream = request.EndGetRequestStream(asynchronousRequest);
			
			// Write to the request stream.
			postStream.Write(myRequestState.requestDataBytes, 0, myRequestState.requestData.Length);
			postStream.Close();
			
			// Start the asynchronous operation to get the response
			request.BeginGetResponse(new AsyncCallback(RespCallback), myRequestState);
		}
		
		private static void RespCallback(IAsyncResult asynchronousResult)
		{  
			try
			{
				// State of request is asynchronous.
				RequestState myRequestState	= (RequestState) asynchronousResult.AsyncState;
				HttpWebRequest request		= myRequestState.request;
				myRequestState.response 	= (HttpWebResponse) request.EndGetResponse(asynchronousResult);
				
				// Read the response into a Stream object.
				Stream responseStream = myRequestState.response.GetResponseStream();
				myRequestState.streamResponse=responseStream;
				
				IAsyncResult asynchronousInputRead = responseStream.BeginRead(myRequestState.BufferRead, 0, BUFFER_SIZE, new AsyncCallback(ReadCallBack), myRequestState);
			}
			catch(WebException e)
			{
			  Debug.Log(e.Message);
			}
		}
		
		private static void ReadCallBack(IAsyncResult asyncResult)
		{
			try
			{
				RequestState myRequestState = (RequestState)asyncResult.AsyncState;
				Stream responseStream = myRequestState.streamResponse;
				int read = responseStream.EndRead( asyncResult );
				
				// Read the HTML page and then do something with it
				if (read > 0)
				{
					myRequestState.responseData.Append(Encoding.UTF8.GetString(myRequestState.BufferRead, 0, read));
					IAsyncResult asynchronousResult = responseStream.BeginRead( myRequestState.BufferRead, 0, BUFFER_SIZE, new AsyncCallback(ReadCallBack), myRequestState);
				}
				else
				{
					if(myRequestState.responseData.Length>1)
					{
						string stringContent = myRequestState.responseData.ToString();
						if (myRequestState.request.Method.ToLower() == "get" && myRequestState.useCache == true) {
							Cache.Save(WWW.EscapeURL(myRequestState.request.RequestUri.ToString()), stringContent);
						}
						myRequestState.response.Close();
						myRequestState.isDone = true;
					}
				
				  	responseStream.Close();
					
					if (myRequestState.callback != null) {
						ResponseCallbackDispatcher.Singleton.requests.Enqueue( myRequestState.owningRequest );
					}
				}
			}
			catch(WebException e)
			{
				Debug.Log(e.Message);
			}
		}
	}
}

