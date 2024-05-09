using Antopia.Domain.DTOs.UserDTOs;
using Antopia.Domain.Entities.ColoniaE;
using Antopia.Domain.Entities.DiaryE;
using Antopia.Domain.Entities.LoginE;
using Antopia.Domain.Entities.NotificacionE;
using Antopia.Domain.Entities.PublicacionE;
using Antopia.Domain.Entities.UserE;
using Microsoft.EntityFrameworkCore;

namespace Antopia.Infrastructure
{
    public class AntopiaDbContext : DbContext
    {
        private readonly string _connection;

        public AntopiaDbContext(string connection)
        {
            _connection = connection;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_connection);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        // Login
        public virtual DbSet<LoginE> LoginEs { get; set; }
        public virtual DbSet<HistorialrefreshtokenE> HistorialrefreshtokenEs { get; set; }
        public virtual DbSet<CodigoRestablecimientoE> CodigoRestablecimientoEs { get; set; }

        // User 
        public virtual DbSet<UserE> UserEs { get; set; }
        public virtual DbSet<FollowersE> FollowersEs { get; set; }
        public virtual DbSet<recomendarFoFollowers> recomendarFoFollowersEs { get; set; }
        public virtual DbSet<LevelE> LevelEs { get; set; }

        // Publication
        public virtual DbSet<PublicationE> PublicationEs { get; set; }
        public virtual DbSet<PublicationImageE> PublicationImageEs { get; set; }
        public virtual DbSet<PublicationVideoE> PublicationVideoEs { get; set; }
        public virtual DbSet<CommentsE> CommentsEs { get; set; }
        public virtual DbSet<CommentsImagenE> CommentsImagenEs { get; set; }
        public virtual DbSet<CommentsLikeE> CommentsLikeEs { get; set; }
        public virtual DbSet<LikeCommentsE> LikeCommentsEs { get; set; }
        public virtual DbSet<AnswerLikeE> AnswerLikeEs { get; set; }
        public virtual DbSet<CommentsAnswercsE> CommentsAnswercsEs { get; set; }
        public virtual DbSet<PublicationReportingE> PublicationReportingEs { get; set; }
        public virtual DbSet<Publication_reporting_reasonE> Publication_reporting_reasonEs { get; set; }
        public virtual DbSet<PublicationShareE> PublicationShareEs { get; set; }

        // Colonia
        public virtual DbSet<ColoniaE> ColoniaEs { get; set; }
        public virtual DbSet<MembersE> MembersEs { get; set; }
        public virtual DbSet<ColoniaPublicationImageE> ColoniaPublicationImageEs { get; set; }
        public virtual DbSet<ColoniaPublicationE> ColoniaPublicationEs { get; set; }

        // Diary
        public virtual DbSet<DiaryE> DiaryEs { get; set; }
        public virtual DbSet<CommentDiaryE> CommentDiaryEs { get; set; }
        public virtual DbSet<DiaryEntriesE> DiaryEntriesEs { get; set; }
        public virtual DbSet<DiaryImageE> DiaryImageEs { get; set; }
        public virtual DbSet<DiaryLikeE> DiaryLikeEs { get; set; }

        //Notificaciones 
        public virtual DbSet<NotificacionE> NotificacionEs { get; set; }
        public virtual DbSet<TypeNotificacionE> TypeNotificacionEs { get; set; }

    }
}