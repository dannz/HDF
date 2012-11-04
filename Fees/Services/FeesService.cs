using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.DomainServices.Hosting;
using System.ServiceModel.DomainServices.Server;
using DataModule.ClubManager;

namespace FeesOnline.Services
{
    [EnableClientAccess]
    public class TeamEntity
    {
        [Key]
        public string Id { get; set; }
        public string TeamName { get; set; }
    }

    [EnableClientAccess]
    public class PlayerEntity
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class FeeInformationEntity
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string RemainingDescription { get; set; }
    }

    // TODO: Create methods containing your application logic.
    [EnableClientAccess()]
    [ServiceContract(Namespace = "HdfService")]
    public class FeesService : DomainService
    {
        public IQueryable<FeeInformationEntity> GetFeeInformation(string playerId)
        {
            List<FeeInformationEntity> data = new List<FeeInformationEntity>();

            using (DataEntities entities = new DataEntities())
            {
                var guid = new Guid(playerId);
                var player = entities.TeamMembers.FirstOrDefault(x => x.Id == guid);

                data.Add(new FeeInformationEntity()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = player.Person.FirstName + " " + player.Person.LastName,
                    RemainingDescription = GetRemainingDescription(player)
                });
            }

            return data.AsQueryable();
        }

        string GetRemainingDescription(TeamMember player)
        {
            if (player.Remaining > 0) return "You have " + player.Remaining.ToString("c") + " remaining to pay";
            if (player.Credit < 0) return "You have " + (-player.Credit).ToString("c") + " credit for next season";

            return "Thanks, your fees are fully paid.";
        }

        public IQueryable<PlayerEntity> GetPlayers(string teamId)
        {
            List<PlayerEntity> data = new List<PlayerEntity>();

            using (DataEntities entities = new DataEntities())
            {
                var guid = new Guid(teamId);

                data = entities.Teams.FirstOrDefault(x => x.Id == guid)
                    .TeamMembers
                    .Where(x => x.Role == PlayerRoles.Casual || x.Role == PlayerRoles.Golie  || x.Role == PlayerRoles.Player)
                    .ToList()
                    .Select(x => new PlayerEntity()
                    {
                        Id = x.Id.ToString(),
                        Name = x.Person.FirstName + " " + x.Person.LastName,
                    })
                    .OrderBy(x => x.Name)
                    .ToList();
            }

            return data.AsQueryable();
        }

        public IQueryable<TeamEntity> GetTeams()
        {
            List<TeamEntity> data = new List<TeamEntity>();

            using (DataEntities entities = new DataEntities())
            {
                int year = entities.CurrentSettings.CurrentYear;

                data = entities.Teams.Where(x => x.Year == year && x.Name != Team.Summer && x.Name != Team.Miscellaneous)
                    .OrderBy(x => x.DisplayOrder)
                    .ToList()
                    .Select(x => new TeamEntity()
                    {
                        Id = x.Id.ToString(),
                        TeamName = x.Name,
                    })
                    .ToList();
            }

            return data.AsQueryable();
        }
    }
}


