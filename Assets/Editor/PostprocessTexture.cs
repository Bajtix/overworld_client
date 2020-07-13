using UnityEditor;
using UnityEngine;
public class PostprocessTexture : AssetPostprocessor
{
    void OnPostprocessTextures(Texture2D texture)
    {
        TextureImporter textureImporter = (TextureImporter)assetImporter;
        textureImporter.alphaIsTransparency = true;
        textureImporter.textureType = TextureImporterType.Sprite;
        textureImporter.SaveAndReimport();
    }
}
