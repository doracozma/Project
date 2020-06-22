using System;

namespace Common
{
    public class UserModel
    {

        public Guid id { get; set; }
        [Encrypted]
        public String name { get; set; }
        [Encrypted]
        public String password { get; set; }
        [Encrypted]
        public String email { get; set; }
        public DateTime creationDate { get; set; }
    }
}
