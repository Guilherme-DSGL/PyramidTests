using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Pyramid.Core
{
    public class TravelSeat
    {
        public int Id { get; private set; }
        public int TravelId { get; private set; }
        public BitArray Bitmap { get; private set; }
        public int ArmchairNumber { get; private set; }

        public TravelSeat(int id, BitArray bitmap, int travelId, int armchairNumber)
        {
            Id = id;
            TravelId = travelId;
            Bitmap = bitmap;
            ArmchairNumber = armchairNumber;
        }

        public TravelSeat(int id, List<Department> departmentsRoute, int travelId, int armchairNumber)
            : this(id, new BitArray(departmentsRoute?.Count ?? throw new ArgumentNullException(nameof(departmentsRoute))), travelId, armchairNumber)
        {}

        public bool IsSeatAvailableFor(int startLocation, int endLocation)
        {
            if (startLocation < 0 || endLocation > Bitmap.Length - 1 || startLocation >= endLocation)
                throw new ArgumentException("Intervalo inválido para verificar disponibilidade." + endLocation + startLocation);

            return !Bitmap.Cast<bool>().Skip(startLocation).Take(endLocation - startLocation).Contains(true);
        }

        public bool IsSeatAvailable()
        {
            return Bitmap.Cast<bool>().Contains(false);
        }

        public void UpdateBitmap(int startLocation, int endLocation)
        {
            if (startLocation < 0 || endLocation > Bitmap.Length - 1 || startLocation >= endLocation)
                throw new ArgumentException("Intervalo inválido para atualizar o bitmap.");

            for (int i = startLocation; i < endLocation; i++)
            {
                Bitmap[i] = true;
            }

            if (endLocation == Bitmap.Length - 1)
            {
                Bitmap[endLocation] = true;
            }
        }
    }
}