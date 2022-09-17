using UnityEngine;

namespace SevenDays.unLOC.Storage
{
    public static class SpriteConverter
    {
        public static SerializableTexture GetTexture(Sprite sprite)
        {
            var texture = sprite.texture;

            return new SerializableTexture
            {
                Width = texture.width,
                Height = texture.height,
                Bytes = texture.EncodeToPNG()
            };
        }

        public static Sprite GetSprite(SerializableTexture serializableTexture)
        {
            var texture = new Texture2D(serializableTexture.Width, serializableTexture.Height,
                TextureFormat.ARGB32, false);

            texture.LoadImage(serializableTexture.Bytes);

            return Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), Vector2.one);
        }
    }
}