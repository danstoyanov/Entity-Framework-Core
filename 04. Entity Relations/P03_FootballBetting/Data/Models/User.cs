using System.Collections.Generic;

namespace P03_FootballBetting.Data.Models
{
    public class User
    {
        public int UserId { get; set; }
        public int Username { get; set; }
        public int Password { get; set; }
        public int Email { get; set; }
        public int Name { get; set; }
        public decimal Balance { get; set; }
        public ICollection<Bet> Bets { get; set; }
    }
}