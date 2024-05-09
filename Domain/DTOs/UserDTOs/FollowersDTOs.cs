using Antopia.Domain.Entities.UserE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antopia.Domain.DTOs.UserDTOs
{
    public class FollowersDTOs
    {
        public int id_followers { get; set; }
        public int id_user { get; set; }
        public int id_follower { get; set; }
        public int isfollower { get; set; }


        public static FollowersDTOs CreateDTO(FollowersE followersE)
        {
            FollowersDTOs followersDTOs = new()
            {
                id_followers = followersE.id_followers,
                id_user = followersE.id_user,
                id_follower = followersE.id_follower,
            };
            return followersDTOs;
        }


        public static FollowersE CreateE(FollowersDTOs followersDTOs)
        {
            FollowersE followersE = new()
            {
                id_followers = followersDTOs.id_followers,
                id_user = followersDTOs.id_user,
                id_follower = followersDTOs.id_follower,

            };
            return followersE;
        }
    }

    public class recomendarFoFollowers
    {
        public int id { get; set; }
        public string userName { get; set; }
    }
}
