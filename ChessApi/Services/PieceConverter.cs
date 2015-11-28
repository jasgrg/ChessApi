using System;
using Newtonsoft.Json;
using ChessApi.Interfaces;
using ChessApi.Services.Pieces;

namespace ChessApi.Services
{
    public class PieceConverter : JsonConverter
    {
        public override bool CanWrite
        {
            get
            {
                return false;
            }
        }
        public override bool CanConvert(Type objectType)
        {
            return objectType.FullName == typeof(IPiece).FullName;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var piece = serializer.Deserialize<Piece>(reader);
            if(piece != null)
            {
                var pc = Piece.GetPiece(piece.PieceType, piece.Player, piece.Location);
                pc.HasMoved = piece.HasMoved;
                return pc;
            }
                
            return piece;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}