namespace Dawem.Enums.Generals
{
    public enum SubscriptionStatus
    {
        Created, // default status after account creation
        Confirmed, // after clicking the verification link sent to email
        Activated, // after payment approval by Dawem Admins
        Deactivated // if subscription finished
    }
}
