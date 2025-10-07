using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoveSoPickNPlace.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialModelsArch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BomEntries",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    JobID = table.Column<Guid>(type: "TEXT", nullable: false),
                    Designator = table.Column<string>(type: "TEXT", nullable: true),
                    PartNumber = table.Column<string>(type: "TEXT", nullable: true),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: true),
                    AssignedFeederSlot = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BomEntries", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CameraFeeds",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Type = table.Column<int>(type: "INTEGER", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: true),
                    StreamEndpoint = table.Column<string>(type: "TEXT", nullable: true),
                    Fps = table.Column<int>(type: "INTEGER", nullable: true),
                    Resolution = table.Column<string>(type: "TEXT", nullable: true),
                    LastFrameAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CameraFeeds", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ComponentDefinitions",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    PackageType = table.Column<string>(type: "TEXT", nullable: true),
                    LengthMm = table.Column<double>(type: "REAL", nullable: true),
                    WidthMm = table.Column<double>(type: "REAL", nullable: true),
                    HeightMm = table.Column<double>(type: "REAL", nullable: true),
                    RotationOffsetDegrees = table.Column<double>(type: "REAL", nullable: true),
                    PickupHeightOffsetMm = table.Column<double>(type: "REAL", nullable: true),
                    TapePitchMm = table.Column<double>(type: "REAL", nullable: true),
                    TapeOrientationCode = table.Column<string>(type: "TEXT", nullable: true),
                    VisionAlignmentRequired = table.Column<bool>(type: "INTEGER", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComponentDefinitions", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "InspectionResults",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    IsCorrect = table.Column<bool>(type: "INTEGER", nullable: true),
                    OffsetX = table.Column<double>(type: "REAL", nullable: true),
                    OffsetY = table.Column<double>(type: "REAL", nullable: true),
                    RotationError = table.Column<double>(type: "REAL", nullable: true),
                    ImagePath = table.Column<string>(type: "TEXT", nullable: true),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InspectionResults", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Jobs",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    CplFileId = table.Column<Guid>(type: "TEXT", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: true),
                    Progress = table.Column<double>(type: "REAL", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CompletedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    EstimatedDuration = table.Column<TimeSpan>(type: "TEXT", nullable: true),
                    Owner = table.Column<string>(type: "TEXT", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "BLOB", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jobs", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "LogEntries",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Level = table.Column<int>(type: "INTEGER", nullable: true),
                    Source = table.Column<string>(type: "TEXT", nullable: true),
                    Message = table.Column<string>(type: "TEXT", nullable: true),
                    JobId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogEntries", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ManualControlCommand",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: true),
                    ParametersJson = table.Column<string>(type: "TEXT", nullable: true),
                    IssuedBy = table.Column<string>(type: "TEXT", nullable: true),
                    IssuedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Executed = table.Column<bool>(type: "INTEGER", nullable: true),
                    ExecutedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManualControlCommand", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    Level = table.Column<int>(type: "INTEGER", nullable: true),
                    Message = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Resolved = table.Column<bool>(type: "INTEGER", nullable: true),
                    ResolvedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "UploadedFiles",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    FileName = table.Column<string>(type: "TEXT", nullable: true),
                    ContentType = table.Column<string>(type: "TEXT", nullable: true),
                    SizeBytes = table.Column<long>(type: "INTEGER", nullable: true),
                    UploadedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    StoragePath = table.Column<string>(type: "TEXT", nullable: true),
                    Parsed = table.Column<bool>(type: "INTEGER", nullable: true),
                    ParsingErrors = table.Column<string>(type: "TEXT", nullable: true),
                    BoardName = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UploadedFiles", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "VisionCalibrations",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    CameraID = table.Column<Guid>(type: "TEXT", nullable: false),
                    IntrinsicsJson = table.Column<string>(type: "TEXT", nullable: true),
                    DistortionJson = table.Column<string>(type: "TEXT", nullable: true),
                    CalibratedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisionCalibrations", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Feeders",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    SlotNumber = table.Column<int>(type: "INTEGER", nullable: true),
                    ComponentDefinitionId = table.Column<Guid>(type: "TEXT", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: true),
                    IsLoaded = table.Column<bool>(type: "INTEGER", nullable: true),
                    RemainingCount = table.Column<int>(type: "INTEGER", nullable: true),
                    LastLoadedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    PickupPosition_X = table.Column<double>(type: "REAL", nullable: true),
                    PickupPosition_Y = table.Column<double>(type: "REAL", nullable: true),
                    PickupPosition_Z = table.Column<double>(type: "REAL", nullable: true),
                    PickupPosition_Rotation = table.Column<double>(type: "REAL", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feeders", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Feeders_ComponentDefinitions_ComponentDefinitionId",
                        column: x => x.ComponentDefinitionId,
                        principalTable: "ComponentDefinitions",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "ComponentPlacements",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    JobID = table.Column<Guid>(type: "TEXT", nullable: false),
                    ComponentDefinitionID = table.Column<Guid>(type: "TEXT", nullable: false),
                    FeederSlot = table.Column<int>(type: "INTEGER", nullable: true),
                    TargetX = table.Column<double>(type: "REAL", nullable: true),
                    TargetY = table.Column<double>(type: "REAL", nullable: true),
                    TargetRotation = table.Column<double>(type: "REAL", nullable: true),
                    PlacedX = table.Column<double>(type: "REAL", nullable: true),
                    PlacedY = table.Column<double>(type: "REAL", nullable: true),
                    PlacedRotation = table.Column<double>(type: "REAL", nullable: true),
                    Success = table.Column<bool>(type: "INTEGER", nullable: true),
                    CorrectionApplied = table.Column<bool>(type: "INTEGER", nullable: true),
                    PlacedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    InspectionResultID = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComponentPlacements", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ComponentPlacements_InspectionResults_InspectionResultID",
                        column: x => x.InspectionResultID,
                        principalTable: "InspectionResults",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_ComponentPlacements_Jobs_JobID",
                        column: x => x.JobID,
                        principalTable: "Jobs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ComponentPlacements_InspectionResultID",
                table: "ComponentPlacements",
                column: "InspectionResultID");

            migrationBuilder.CreateIndex(
                name: "IX_ComponentPlacements_JobID",
                table: "ComponentPlacements",
                column: "JobID");

            migrationBuilder.CreateIndex(
                name: "IX_Feeders_ComponentDefinitionId",
                table: "Feeders",
                column: "ComponentDefinitionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BomEntries");

            migrationBuilder.DropTable(
                name: "CameraFeeds");

            migrationBuilder.DropTable(
                name: "ComponentPlacements");

            migrationBuilder.DropTable(
                name: "Feeders");

            migrationBuilder.DropTable(
                name: "LogEntries");

            migrationBuilder.DropTable(
                name: "ManualControlCommand");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "UploadedFiles");

            migrationBuilder.DropTable(
                name: "VisionCalibrations");

            migrationBuilder.DropTable(
                name: "InspectionResults");

            migrationBuilder.DropTable(
                name: "Jobs");

            migrationBuilder.DropTable(
                name: "ComponentDefinitions");
        }
    }
}
