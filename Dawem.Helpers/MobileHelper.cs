namespace Dawem.Helpers
{
    public static class MobileHelper
    {
        public static string HandleMobile(string MobileNumber)
        {
            MobileNumber = MobileNumber.StartsWith("00966") ?
                               MobileNumber.Remove(0, 5) : MobileNumber;
            MobileNumber = MobileNumber.StartsWith("0020") ?
                               MobileNumber.Remove(0, 4) : MobileNumber;

            MobileNumber = MobileNumber.StartsWith("0966") || MobileNumber.StartsWith("+966") ?
               MobileNumber.Remove(0, 4) : MobileNumber;
            MobileNumber = MobileNumber.StartsWith("020") || MobileNumber.StartsWith("+20") ?
               MobileNumber.Remove(0, 3) : MobileNumber;



            MobileNumber = MobileNumber.StartsWith("966") ? MobileNumber.Remove(0, 3) : MobileNumber;
            MobileNumber = MobileNumber.StartsWith("20") ? MobileNumber.Remove(0, 2) : MobileNumber;

            MobileNumber = MobileNumber.StartsWith("0") ?
                MobileNumber.Replace(" ", DawemKeys.EmptyString) : "0" + MobileNumber.Replace(" ", DawemKeys.EmptyString);
            return MobileNumber;
        }

    }
}

