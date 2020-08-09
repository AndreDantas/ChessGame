using Chess.Models.Constants;
using Chess.Models.Game;
using Chess.Models.Game.Action;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Chess.Serialization
{
    public class ActionConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(IChessAction));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var token = JToken.Load(reader);

            if (!token.HasValues || token["Name"] == null)
                return null;

            switch (token["Name"].Value<string>())
            {
                case Actions.MOVE:
                    return token.ToObject<Move>(serializer);

                case Actions.SPAWN_PIECE:
                    return token.ToObject<SpawnPiece>(serializer);

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