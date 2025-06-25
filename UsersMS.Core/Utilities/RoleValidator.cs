namespace UsersMS.Domain.Utilities
{
    public static class RoleValidator
    {
        public static bool CanPerformAction(string role, string targetRole)
        {
            if (role == "Administrator")
            {
                return true;
            }

            if (role == "Auctioneer" && targetRole == "Bidder")
            {
                return true;
            }

            if (role == "TechnicalSupport" && (targetRole == "Auctioneer" || targetRole == "Bidder"))
            {
                return true;
            }

            return false;
        }
    }
}
