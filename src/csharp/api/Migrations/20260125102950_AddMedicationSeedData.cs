using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class AddMedicationSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_medication_name",
                table: "medication");

            migrationBuilder.CreateIndex(
                name: "ix_medication_name",
                table: "medication",
                column: "name",
                unique: true);

            migrationBuilder.Sql(@"
INSERT INTO medication (name, manufacturer, created_by, created_at, updated_by, updated_at) VALUES
('斯斯鼻炎藥', '五洲製藥股份有限公司', 'system', '2026-01-25 10:01:48.809906+00', 'system', '2026-01-25 10:01:48.809906+00')
ON CONFLICT (name) DO NOTHING;
INSERT INTO medication (name, manufacturer, created_by, created_at, updated_by, updated_at) VALUES
('斯斯感冒藥', '五洲製藥股份有限公司', 'system', '2026-01-25 10:01:48.809906+00', 'system', '2026-01-25 10:01:48.809906+00')
ON CONFLICT (name) DO NOTHING;
INSERT INTO medication (name, manufacturer, created_by, created_at, updated_by, updated_at) VALUES
('斯斯喉嚨藥', '五洲製藥股份有限公司', 'system', '2026-01-25 10:01:48.809906+00', 'system', '2026-01-25 10:01:48.809906+00')
ON CONFLICT (name) DO NOTHING;
INSERT INTO medication (name, manufacturer, created_by, created_at, updated_by, updated_at) VALUES
('威克倦', 'Bausch Health (加拿大)', 'system', '2026-01-25 10:06:01.674412+00', 'system', '2026-01-25 10:06:01.674412+00')
ON CONFLICT (name) DO NOTHING;
INSERT INTO medication (name, manufacturer, created_by, created_at, updated_by, updated_at) VALUES
('恩特萊', 'Corden Pharma (德國)', 'system', '2026-01-25 10:06:01.674412+00', 'system', '2026-01-25 10:06:01.674412+00')
ON CONFLICT (name) DO NOTHING;
INSERT INTO medication (name, manufacturer, created_by, created_at, updated_by, updated_at) VALUES
('離憂', '中國化學製藥 (中化)', 'system', '2026-01-25 10:06:01.674412+00', 'system', '2026-01-25 10:06:01.674412+00')
ON CONFLICT (name) DO NOTHING;
INSERT INTO medication (name, manufacturer, created_by, created_at, updated_by, updated_at) VALUES
('戀多眠', '健喬信元醫藥生技', 'system', '2026-01-25 10:06:01.674412+00', 'system', '2026-01-25 10:06:01.674412+00')
ON CONFLICT (name) DO NOTHING;
INSERT INTO medication (name, manufacturer, created_by, created_at, updated_by, updated_at) VALUES
('贊安諾', '輝瑞大藥廠 (美國)', 'system', '2026-01-25 10:06:01.674412+00', 'system', '2026-01-25 10:06:01.674412+00')
ON CONFLICT (name) DO NOTHING;
INSERT INTO medication (name, manufacturer, created_by, created_at, updated_by, updated_at) VALUES
('利福全', 'Recipharm (西班牙)', 'system', '2026-01-25 10:06:01.674412+00', 'system', '2026-01-25 10:06:01.674412+00')
ON CONFLICT (name) DO NOTHING;
INSERT INTO medication (name, manufacturer, created_by, created_at, updated_by, updated_at) VALUES
('帝拔癲', 'Sanofi (賽諾菲，法國)', 'system', '2026-01-25 10:06:01.674412+00', 'system', '2026-01-25 10:06:01.674412+00')
ON CONFLICT (name) DO NOTHING;
INSERT INTO medication (name, manufacturer, created_by, created_at, updated_by, updated_at) VALUES
('導美睡', 'Recipharm (西班牙)', 'system', '2026-01-25 10:06:01.674412+00', 'system', '2026-01-25 10:06:01.674412+00')
ON CONFLICT (name) DO NOTHING;
INSERT INTO medication (name, manufacturer, created_by, created_at, updated_by, updated_at) VALUES
('美得眠', '瑞士藥廠股份有限公司', 'system', '2026-01-25 10:07:28.327671+00', 'system', '2026-01-25 10:07:28.327671+00')
ON CONFLICT (name) DO NOTHING;
INSERT INTO medication (name, manufacturer, created_by, created_at, updated_by, updated_at) VALUES
('使蒂諾斯', 'Sanofi Winthrop (法國)', 'system', '2026-01-25 10:07:28.327671+00', 'system', '2026-01-25 10:07:28.327671+00')
ON CONFLICT (name) DO NOTHING;
INSERT INTO medication (name, manufacturer, created_by, created_at, updated_by, updated_at) VALUES
('普拿疼', '葛蘭素史克(GSK)', 'system', '2026-01-25 10:16:16.816139+00', 'system', '2026-01-25 10:16:16.816139+00')
ON CONFLICT (name) DO NOTHING;
INSERT INTO medication (name, manufacturer, created_by, created_at, updated_by, updated_at) VALUES
('吉胃福適', '保寧製藥', 'system', '2026-01-25 10:16:16.816139+00', 'system', '2026-01-25 10:16:16.816139+00')
ON CONFLICT (name) DO NOTHING;
INSERT INTO medication (name, manufacturer, created_by, created_at, updated_by, updated_at) VALUES
('金十字胃腸藥', '新萬仁化學製藥', 'system', '2026-01-25 10:16:16.816139+00', 'system', '2026-01-25 10:16:16.816139+00')
ON CONFLICT (name) DO NOTHING;
INSERT INTO medication (name, manufacturer, created_by, created_at, updated_by, updated_at) VALUES
('大正百保能', '大正製藥', 'system', '2026-01-25 10:16:16.816139+00', 'system', '2026-01-25 10:16:16.816139+00')
ON CONFLICT (name) DO NOTHING;
INSERT INTO medication (name, manufacturer, created_by, created_at, updated_by, updated_at) VALUES
('拜耳阿斯匹靈', '拜耳(Bayer)', 'system', '2026-01-25 10:16:16.816139+00', 'system', '2026-01-25 10:16:16.816139+00')
ON CONFLICT (name) DO NOTHING;
INSERT INTO medication (name, manufacturer, created_by, created_at, updated_by, updated_at) VALUES
('龍角散', '株式會社龍角散', 'system', '2026-01-25 10:16:16.816139+00', 'system', '2026-01-25 10:16:16.816139+00')
ON CONFLICT (name) DO NOTHING;
INSERT INTO medication (name, manufacturer, created_by, created_at, updated_by, updated_at) VALUES
('伏冒熱飲', '葛蘭素史克(GSK)', 'system', '2026-01-25 10:16:16.816139+00', 'system', '2026-01-25 10:16:16.816139+00')
ON CONFLICT (name) DO NOTHING;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_medication_name",
                table: "medication");

            migrationBuilder.CreateIndex(
                name: "ix_medication_name",
                table: "medication",
                column: "name");
        }
    }
}
