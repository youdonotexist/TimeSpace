using UnityEngine;
using System.Collections;

public static class tk2dBaseSpriteExtension {
	
	public static void SetSize(this tk2dBaseSprite sprite, Vector2 pixelSize) {
		tk2dSpriteDefinition def = sprite.CurrentSprite;
		Vector2 requestedSize = new Vector2( def.texelSize.x * pixelSize.x, def.texelSize.y * pixelSize.y );
		Vector3 dimensions = def.untrimmedBoundsData[1];
		Vector3 scale = new Vector3( requestedSize.x / dimensions.x, requestedSize.y / dimensions.y, 1 );
		sprite.scale = scale;
	}
	
	public static Vector2 GetSourcePixelSize( this tk2dBaseSprite sprite ) {
		tk2dSpriteDefinition def = sprite.CurrentSprite;
		Vector3 dims = def.untrimmedBoundsData[1];
		return new Vector2( dims.x / def.texelSize.x, dims.y / def.texelSize.y );
	}
}

