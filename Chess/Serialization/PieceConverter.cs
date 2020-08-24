using Chess.Models.Constants;
using Chess.Models.Pieces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Chess.Serialization
{
    public class PieceConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(ChessPiece));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var token = JToken.Load(reader);

            if (!token.HasValues || token["Name"] == null)
                return null;

            switch (token["Name"].Value<string>())
            {
                case ChessPieces.PAWN:
                    return token.ToObject<Pawn>(serializer);

                case ChessPieces.KNIGHT:
                    return token.ToObject<Knight>(serializer);

                case ChessPieces.ROOK:
                    return token.ToObject<Rook>(serializer);

                case ChessPieces.BISHOP:
                    return token.ToObject<Bishop>(serializer);

                case ChessPieces.QUEEN:
                    return token.ToObject<Queen>(serializer);

                case ChessPieces.KING:
                    return token.ToObject<King>(serializer);

                default:
                    return null;
            }
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}