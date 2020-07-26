using Chess.Models.Constants;
using Chess.Models.Pieces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chess.Serialization
{
    public class PieceConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(Piece));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var token = JToken.Load(reader);

            if (!token.HasValues || token["Name"] == null)
                return null;

            switch (token["Name"].Value<string>())
            {
                case Pieces.PAWN:
                    return token.ToObject<Pawn>(serializer);

                case Pieces.KNIGHT:
                    return token.ToObject<Knight>(serializer);

                case Pieces.ROOK:
                    return token.ToObject<Rook>(serializer);

                case Pieces.BISHOP:
                    return token.ToObject<Bishop>(serializer);

                case Pieces.QUEEN:
                    return token.ToObject<Queen>(serializer);

                case Pieces.KING:
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