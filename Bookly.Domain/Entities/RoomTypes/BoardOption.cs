using Bookly.Domain.Entities.Base;
using Bookly.Domain.Enums;

namespace Bookly.Domain.Entities.RoomTypes
{
    public class BoardOption : BaseEntity
    {
        public int RoomTypeId { get; private set; }
        public BoardType Type { get; private set; }
        public decimal ExtraPricePerNight { get; private set; }
        public bool IsActive { get; private set; } = true;
        private BoardOption() { }

        public BoardOption(int roomTypeId, BoardType type, decimal extraPricePerNight)
        {
            if (roomTypeId <= 0)
                throw new ArgumentOutOfRangeException(nameof(roomTypeId));
            if (extraPricePerNight < 0)
                throw new ArgumentOutOfRangeException(nameof(extraPricePerNight), "Extra price cannot be negative.");

            RoomTypeId = roomTypeId;
            Type = type;
            ExtraPricePerNight = extraPricePerNight;
            IsActive = true;
        }
        public void Deactivate() => IsActive = false;
        public void Activate() => IsActive = true;


    }
}
