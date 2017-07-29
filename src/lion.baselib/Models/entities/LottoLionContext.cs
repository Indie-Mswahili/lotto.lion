using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;

namespace LottoLion.BaseLib.Models.Entity
{
    public partial class LottoLionContext : DbContext
    {
        private string __connection_string;

        public LottoLionContext(string connectionstring)
        {
            __connection_string = connectionstring;
        }

        public LottoLionContext(DbContextOptions<LottoLionContext> options)
        : base(options)
        {
            __connection_string = this.Database.GetDbConnection().ConnectionString;
        }

        public virtual DbSet<TbLionAnalysis> TbLionAnalysis { get; set; }
        public virtual DbSet<TbLionChoice> TbLionChoice { get; set; }
        public virtual DbSet<TbLionFactor> TbLionFactor { get; set; }
        public virtual DbSet<TbLionJackpot> TbLionJackpot { get; set; }
        public virtual DbSet<TbLionMember> TbLionMember { get; set; }
        public virtual DbSet<TbLionNotify> TbLionNotify { get; set; }
        public virtual DbSet<TbLionPercent> TbLionPercent { get; set; }
        public virtual DbSet<TbLionSelect> TbLionSelect { get; set; }
        public virtual DbSet<TbLionWinner> TbLionWinner { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            foreach (var ext in optionsBuilder.Options.Extensions)
            {
                if (ext is NpgsqlOptionsExtension)
                {
                    if (string.IsNullOrEmpty((ext as NpgsqlOptionsExtension).ConnectionString) == false)
                        return;
                }
            }

            optionsBuilder.UseNpgsql(__connection_string);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TbLionAnalysis>(entity =>
            {
                entity.HasKey(e => e.SequenceNo)
                    .HasName("PK_tb_lion_analysis");

                entity.ToTable("tb_lion_analysis");

                entity.Property(e => e.SequenceNo)
                    .HasColumnName("sequence_no")
                    .ValueGeneratedNever();

                entity.Property(e => e.Digit1).HasColumnName("digit1");

                entity.Property(e => e.Digit2).HasColumnName("digit2");

                entity.Property(e => e.Digit3).HasColumnName("digit3");

                entity.Property(e => e.Digit4).HasColumnName("digit4");

                entity.Property(e => e.Digit5).HasColumnName("digit5");

                entity.Property(e => e.Digit6).HasColumnName("digit6");

                entity.Property(e => e.Digit7).HasColumnName("digit7");

                entity.Property(e => e.EdgeGroup)
                    .HasColumnName("edge_group")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.EdgeHighLow)
                    .HasColumnName("edge_high_low")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.EdgeNumber)
                    .HasColumnName("edge_number")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Group18)
                    .HasColumnName("group18")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Group27)
                    .HasColumnName("group27")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Group36)
                    .HasColumnName("group36")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Group45)
                    .HasColumnName("group45")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Group9)
                    .HasColumnName("group9")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.HighLow)
                    .HasColumnName("high_low")
                    .HasColumnType("varchar")
                    .HasMaxLength(32);

                entity.Property(e => e.HotCold1)
                    .HasColumnName("hot_cold1")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.HotCold2)
                    .HasColumnName("hot_cold2")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.HotCold3)
                    .HasColumnName("hot_cold3")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.HotCold4)
                    .HasColumnName("hot_cold4")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.HotCold5)
                    .HasColumnName("hot_cold5")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.HotCold6)
                    .HasColumnName("hot_cold6")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.HotColdAvg)
                    .HasColumnName("hot_cold_avg")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.HotColdL10)
                    .HasColumnName("hot_cold_l10")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.HotColdSum)
                    .HasColumnName("hot_cold_sum")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.NoDigits10)
                    .HasColumnName("no_digits10")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.NoDigits3)
                    .HasColumnName("no_digits3")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.NoDigits5)
                    .HasColumnName("no_digits5")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.NoWinner10)
                    .HasColumnName("no_winner10")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.NoWinner3)
                    .HasColumnName("no_winner3")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.NoWinner5)
                    .HasColumnName("no_winner5")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.OddEven)
                    .HasColumnName("odd_even")
                    .HasColumnType("varchar")
                    .HasMaxLength(32);

                entity.Property(e => e.SameJackpot4)
                    .HasColumnName("same_jackpot4")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.SameJackpot5)
                    .HasColumnName("same_jackpot5")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.SameJackpot6)
                    .HasColumnName("same_jackpot6")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.SeriesNumber)
                    .HasColumnName("series_number")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.SumAppearance)
                    .HasColumnName("sum_appearance")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.SumBalance)
                    .HasColumnName("sum_balance")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.SumEven)
                    .HasColumnName("sum_even")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.SumFrequence)
                    .HasColumnName("sum_frequence")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.SumHigh)
                    .HasColumnName("sum_high")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.SumLow)
                    .HasColumnName("sum_low")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.SumOdd)
                    .HasColumnName("sum_odd")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.WinChart0)
                    .HasColumnName("win_chart0")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.WinChart1)
                    .HasColumnName("win_chart1")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.WinChart2)
                    .HasColumnName("win_chart2")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.WinChart3)
                    .HasColumnName("win_chart3")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.WinChart4)
                    .HasColumnName("win_chart4")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.WinEven)
                    .HasColumnName("win_even")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.WinHigh)
                    .HasColumnName("win_high")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.WinLow)
                    .HasColumnName("win_low")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.WinOdd)
                    .HasColumnName("win_odd")
                    .HasDefaultValueSql("0");
            });

            modelBuilder.Entity<TbLionChoice>(entity =>
            {
                entity.HasKey(e => new { e.SequenceNo, e.LoginId, e.ChoiceNo })
                    .HasName("PK_tb_lion_choice");

                entity.ToTable("tb_lion_choice");

                entity.HasIndex(e => new { e.SequenceNo, e.LoginId })
                    .HasName("ix_lion_choice");

                entity.Property(e => e.SequenceNo).HasColumnName("sequence_no");

                entity.Property(e => e.LoginId)
                    .HasColumnName("login_id")
                    .HasColumnType("varchar")
                    .HasMaxLength(32);

                entity.Property(e => e.ChoiceNo).HasColumnName("choice_no");

                entity.Property(e => e.Amount).HasColumnName("amount");

                entity.Property(e => e.Digit1).HasColumnName("digit1");

                entity.Property(e => e.Digit2).HasColumnName("digit2");

                entity.Property(e => e.Digit3).HasColumnName("digit3");

                entity.Property(e => e.Digit4).HasColumnName("digit4");

                entity.Property(e => e.Digit5).HasColumnName("digit5");

                entity.Property(e => e.Digit6).HasColumnName("digit6");

                entity.Property(e => e.IsMailSent)
                    .HasColumnName("is_mail_sent")
                    .HasDefaultValueSql("false");

                entity.Property(e => e.Ranking).HasColumnName("ranking");

                entity.Property(e => e.Remark)
                    .HasColumnName("remark")
                    .HasColumnType("varchar")
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<TbLionFactor>(entity =>
            {
                entity.HasKey(e => e.SequenceNo)
                    .HasName("PK_tb_lion_factor");

                entity.ToTable("tb_lion_factor");

                entity.Property(e => e.SequenceNo)
                    .HasColumnName("sequence_no")
                    .ValueGeneratedNever();

                entity.Property(e => e.HighSelection)
                    .HasColumnName("high_selection")
                    .HasDefaultValueSql("false");

                entity.Property(e => e.LNoCombination)
                    .HasColumnName("l_no_combination")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.LNoExtraction)
                    .HasColumnName("l_no_extraction")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.LNoSampling)
                    .HasColumnName("l_no_sampling")
                    .HasDefaultValueSql("27");

                entity.Property(e => e.MaxGroupBalance)
                    .HasColumnName("max_group_balance")
                    .HasDefaultValueSql("3");

                entity.Property(e => e.MaxHighLow)
                    .HasColumnName("max_high_low")
                    .HasDefaultValueSql("5");

                entity.Property(e => e.MaxLastNumber)
                    .HasColumnName("max_last_number")
                    .HasDefaultValueSql("3");

                entity.Property(e => e.MaxOddEven)
                    .HasColumnName("max_odd_even")
                    .HasDefaultValueSql("5");

                entity.Property(e => e.MaxSameDigits)
                    .HasColumnName("max_same_digits")
                    .HasDefaultValueSql("4");

                entity.Property(e => e.MaxSameJackpot)
                    .HasColumnName("max_same_jackpot")
                    .HasDefaultValueSql("2");

                entity.Property(e => e.MaxSeriesNumber)
                    .HasColumnName("max_series_number")
                    .HasDefaultValueSql("3");

                entity.Property(e => e.MaxSumFrequence)
                    .HasColumnName("max_sum_frequence")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.MinHighLow)
                    .HasColumnName("min_high_low")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.MinOddEven)
                    .HasColumnName("min_odd_even")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.NoJackpot1)
                    .HasColumnName("no_jackpot1")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.NoJackpot2)
                    .HasColumnName("no_jackpot2")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.NoJackpot3)
                    .HasColumnName("no_jackpot3")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.NoJackpot4)
                    .HasColumnName("no_jackpot4")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.NoJackpot5)
                    .HasColumnName("no_jackpot5")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.OddSelection)
                    .HasColumnName("odd_selection")
                    .HasDefaultValueSql("false");

                entity.Property(e => e.RNoCombination)
                    .HasColumnName("r_no_combination")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.RNoExtraction)
                    .HasColumnName("r_no_extraction")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.RNoSampling)
                    .HasColumnName("r_no_sampling")
                    .HasDefaultValueSql("27");

                entity.Property(e => e.WinningAmount1)
                    .HasColumnName("winning_amount1")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.WinningAmount2)
                    .HasColumnName("winning_amount2")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.WinningAmount3)
                    .HasColumnName("winning_amount3")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.WinningAmount4)
                    .HasColumnName("winning_amount4")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.WinningAmount5)
                    .HasColumnName("winning_amount5")
                    .HasDefaultValueSql("0");
            });

            modelBuilder.Entity<TbLionJackpot>(entity =>
            {
                entity.HasKey(e => new { e.SequenceNo, e.LoginId })
                    .HasName("PK_tb_lion_jackpot");

                entity.ToTable("tb_lion_jackpot");

                entity.Property(e => e.SequenceNo).HasColumnName("sequence_no");

                entity.Property(e => e.LoginId)
                    .HasColumnName("login_id")
                    .HasColumnType("varchar")
                    .HasMaxLength(32);

                entity.Property(e => e.Digit1).HasColumnName("digit1");

                entity.Property(e => e.Digit2).HasColumnName("digit2");

                entity.Property(e => e.Digit3).HasColumnName("digit3");

                entity.Property(e => e.NoChoice)
                    .HasColumnName("no_choice")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.NoJackpot)
                    .HasColumnName("no_jackpot")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.WinningAmount)
                    .HasColumnName("winning_amount")
                    .HasDefaultValueSql("0");
            });

            modelBuilder.Entity<TbLionMember>(entity =>
            {
                entity.HasKey(e => e.LoginId)
                    .HasName("PK_tb_lion_member");

                entity.ToTable("tb_lion_member");

                entity.HasIndex(e => e.EmailAddress)
                    .HasName("ix_lion_member")
                    .IsUnique();

                entity.Property(e => e.LoginId)
                    .HasColumnName("login_id")
                    .HasColumnType("varchar")
                    .HasMaxLength(32);

                entity.Property(e => e.AccessToken)
                    .IsRequired()
                    .HasColumnName("access_token")
                    .HasColumnType("varchar")
                    .HasMaxLength(256);

                entity.Property(e => e.CreateTime)
                    .HasColumnName("create_time")
                    .HasColumnType("timestamptz")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.DeviceId)
                    .IsRequired()
                    .HasColumnName("device_id")
                    .HasColumnType("varchar")
                    .HasMaxLength(256);

                entity.Property(e => e.DeviceType)
                    .IsRequired()
                    .HasColumnName("device_type")
                    .HasColumnType("varchar")
                    .HasMaxLength(1)
                    .HasDefaultValueSql("'U'::character varying");

                entity.Property(e => e.Digit1).HasColumnName("digit1");

                entity.Property(e => e.Digit2).HasColumnName("digit2");

                entity.Property(e => e.Digit3).HasColumnName("digit3");

                entity.Property(e => e.EmailAddress)
                    .IsRequired()
                    .HasColumnName("email_address")
                    .HasColumnType("varchar")
                    .HasMaxLength(64);

                entity.Property(e => e.IsAlive)
                    .HasColumnName("is_alive")
                    .HasDefaultValueSql("false");

                entity.Property(e => e.IsDirectSend)
                    .HasColumnName("is_direct_send")
                    .HasDefaultValueSql("false");

                entity.Property(e => e.IsMailSend)
                    .HasColumnName("is_mail_send")
                    .HasDefaultValueSql("false");

                entity.Property(e => e.IsNumberChoice)
                    .HasColumnName("is_number_choice")
                    .HasDefaultValueSql("false");

                entity.Property(e => e.LoginName)
                    .IsRequired()
                    .HasColumnName("login_name")
                    .HasColumnType("varchar")
                    .HasMaxLength(64);

                entity.Property(e => e.LoginPassword)
                    .IsRequired()
                    .HasColumnName("login_password")
                    .HasColumnType("varchar")
                    .HasMaxLength(63);

                entity.Property(e => e.MailError)
                    .HasColumnName("mail_error")
                    .HasDefaultValueSql("false");

                entity.Property(e => e.MaxSelectNumber)
                    .HasColumnName("max_select_number")
                    .HasDefaultValueSql("10");

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasColumnName("phone_number")
                    .HasColumnType("varchar")
                    .HasMaxLength(32);

                entity.Property(e => e.Remark)
                    .HasColumnName("remark")
                    .HasColumnType("varchar")
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<TbLionNotify>(entity =>
            {
                entity.HasKey(e => new { e.LoginId, e.NotifyTime })
                    .HasName("PK_tb_lion_notify");

                entity.ToTable("tb_lion_notify");

                entity.HasIndex(e => new { e.LoginId, e.IsRead })
                    .HasName("ix_lion_notify");

                entity.Property(e => e.LoginId)
                    .HasColumnName("login_id")
                    .HasColumnType("varchar")
                    .HasMaxLength(32);

                entity.Property(e => e.NotifyTime)
                    .HasColumnName("notify_time")
                    .HasColumnType("timestamptz");

                entity.Property(e => e.IsRead)
                    .HasColumnName("is_read")
                    .HasDefaultValueSql("false");

                entity.Property(e => e.Message)
                    .IsRequired()
                    .HasColumnName("message")
                    .HasColumnType("varchar")
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<TbLionPercent>(entity =>
            {
                entity.HasKey(e => e.SequenceNo)
                    .HasName("PK_tb_lion_percent");

                entity.ToTable("tb_lion_percent");

                entity.Property(e => e.SequenceNo)
                    .HasColumnName("sequence_no")
                    .ValueGeneratedNever();

                entity.Property(e => e.LJackpot01)
                    .HasColumnName("l_jackpot_01")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.LJackpot02)
                    .HasColumnName("l_jackpot_02")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.LJackpot03)
                    .HasColumnName("l_jackpot_03")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.LJackpot04)
                    .HasColumnName("l_jackpot_04")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.LJackpot05)
                    .HasColumnName("l_jackpot_05")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.LJackpot06)
                    .HasColumnName("l_jackpot_06")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.LJackpot07)
                    .HasColumnName("l_jackpot_07")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.LJackpot08)
                    .HasColumnName("l_jackpot_08")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.LJackpot09)
                    .HasColumnName("l_jackpot_09")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.LJackpot10)
                    .HasColumnName("l_jackpot_10")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.LSelect01)
                    .HasColumnName("l_select_01")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.LSelect02)
                    .HasColumnName("l_select_02")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.LSelect03)
                    .HasColumnName("l_select_03")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.LSelect04)
                    .HasColumnName("l_select_04")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.LSelect05)
                    .HasColumnName("l_select_05")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.LSelect06)
                    .HasColumnName("l_select_06")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.LSelect07)
                    .HasColumnName("l_select_07")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.LSelect08)
                    .HasColumnName("l_select_08")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.LSelect09)
                    .HasColumnName("l_select_09")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.LSelect10)
                    .HasColumnName("l_select_10")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.RJackpot01)
                    .HasColumnName("r_jackpot_01")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.RJackpot02)
                    .HasColumnName("r_jackpot_02")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.RJackpot03)
                    .HasColumnName("r_jackpot_03")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.RJackpot04)
                    .HasColumnName("r_jackpot_04")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.RJackpot05)
                    .HasColumnName("r_jackpot_05")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.RJackpot06)
                    .HasColumnName("r_jackpot_06")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.RJackpot07)
                    .HasColumnName("r_jackpot_07")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.RJackpot08)
                    .HasColumnName("r_jackpot_08")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.RJackpot09)
                    .HasColumnName("r_jackpot_09")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.RJackpot10)
                    .HasColumnName("r_jackpot_10")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.RSelect01)
                    .HasColumnName("r_select_01")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.RSelect02)
                    .HasColumnName("r_select_02")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.RSelect03)
                    .HasColumnName("r_select_03")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.RSelect04)
                    .HasColumnName("r_select_04")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.RSelect05)
                    .HasColumnName("r_select_05")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.RSelect06)
                    .HasColumnName("r_select_06")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.RSelect07)
                    .HasColumnName("r_select_07")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.RSelect08)
                    .HasColumnName("r_select_08")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.RSelect09)
                    .HasColumnName("r_select_09")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.RSelect10)
                    .HasColumnName("r_select_10")
                    .HasDefaultValueSql("0");
            });

            modelBuilder.Entity<TbLionSelect>(entity =>
            {
                entity.HasKey(e => new { e.SequenceNo, e.SelectNo })
                    .HasName("PK_tb_lion_select");

                entity.ToTable("tb_lion_select");

                entity.HasIndex(e => new { e.SequenceNo, e.SelectNo, e.IsUsed })
                    .HasName("ix_lion_select")
                    .IsUnique();

                entity.Property(e => e.SequenceNo).HasColumnName("sequence_no");

                entity.Property(e => e.SelectNo).HasColumnName("select_no");

                entity.Property(e => e.Amount).HasColumnName("amount");

                entity.Property(e => e.Digit1).HasColumnName("digit1");

                entity.Property(e => e.Digit2).HasColumnName("digit2");

                entity.Property(e => e.Digit3).HasColumnName("digit3");

                entity.Property(e => e.Digit4).HasColumnName("digit4");

                entity.Property(e => e.Digit5).HasColumnName("digit5");

                entity.Property(e => e.Digit6).HasColumnName("digit6");

                entity.Property(e => e.IsLeft)
                    .HasColumnName("is_left")
                    .HasDefaultValueSql("false");

                entity.Property(e => e.IsUsed)
                    .HasColumnName("is_used")
                    .HasDefaultValueSql("false");

                entity.Property(e => e.Ranking).HasColumnName("ranking");

                entity.Property(e => e.Remark)
                    .HasColumnName("remark")
                    .HasColumnType("varchar")
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<TbLionWinner>(entity =>
            {
                entity.HasKey(e => e.SequenceNo)
                    .HasName("PK_tb_lion_winner");

                entity.ToTable("tb_lion_winner");

                entity.Property(e => e.SequenceNo)
                    .HasColumnName("sequence_no")
                    .ValueGeneratedNever();

                entity.Property(e => e.Amount1).HasColumnName("amount1");

                entity.Property(e => e.Amount2).HasColumnName("amount2");

                entity.Property(e => e.Amount3).HasColumnName("amount3");

                entity.Property(e => e.Amount4).HasColumnName("amount4");

                entity.Property(e => e.Amount5).HasColumnName("amount5");

                entity.Property(e => e.AutoSelect).HasColumnName("auto_select");

                entity.Property(e => e.Count1).HasColumnName("count1");

                entity.Property(e => e.Count2).HasColumnName("count2");

                entity.Property(e => e.Count3).HasColumnName("count3");

                entity.Property(e => e.Count4).HasColumnName("count4");

                entity.Property(e => e.Count5).HasColumnName("count5");

                entity.Property(e => e.Digit1).HasColumnName("digit1");

                entity.Property(e => e.Digit2).HasColumnName("digit2");

                entity.Property(e => e.Digit3).HasColumnName("digit3");

                entity.Property(e => e.Digit4).HasColumnName("digit4");

                entity.Property(e => e.Digit5).HasColumnName("digit5");

                entity.Property(e => e.Digit6).HasColumnName("digit6");

                entity.Property(e => e.Digit7).HasColumnName("digit7");

                entity.Property(e => e.IssueDate)
                    .HasColumnName("issue_date")
                    .HasColumnType("timestamptz")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.PaymentDate)
                    .HasColumnName("payment_date")
                    .HasColumnType("timestamptz")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Remark)
                    .HasColumnName("remark")
                    .HasColumnType("varchar")
                    .HasMaxLength(256);
            });
        }
    }
}