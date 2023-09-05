﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Protocols.TestManager.PTMService.DatabaseMigration;

#nullable disable

namespace Microsoft.Protocols.TestManager.PTMService.DatabaseMigration.Migrations
{
    [DbContext(typeof(PTMServiceDbContextForMigration))]
    [Migration("20230621130528_SupportCapabilitiesConfiguration")]
    partial class SupportCapabilitiesConfiguration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.5");

            modelBuilder.Entity("Microsoft.Protocols.TestManager.PTMService.Common.Entities.CapabilitiesConfig", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Path")
                        .HasColumnType("TEXT");

                    b.Property<string>("TestSuiteName")
                        .HasColumnType("TEXT");

                    b.Property<string>("TestSuiteVersion")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("CapabilitiesConfigs");
                });

            modelBuilder.Entity("Microsoft.Protocols.TestManager.PTMService.Common.Entities.TestResult", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<int?>("Failed")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Inconclusive")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("NotRun")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Passed")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Path")
                        .HasColumnType("TEXT");

                    b.Property<int>("State")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TestSuiteConfigurationId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Total")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("TestSuiteConfigurationId");

                    b.ToTable("TestResults");
                });

            modelBuilder.Entity("Microsoft.Protocols.TestManager.PTMService.Common.Entities.TestSuiteConfiguration", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Path")
                        .HasColumnType("TEXT");

                    b.Property<int>("TestSuiteId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("TestSuiteId");

                    b.ToTable("TestSuiteConfigurations");
                });

            modelBuilder.Entity("Microsoft.Protocols.TestManager.PTMService.Common.Entities.TestSuiteInstallation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<int>("InstallMethod")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Path")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Removed")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Version")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("TestSuiteInstallations");
                });

            modelBuilder.Entity("Microsoft.Protocols.TestManager.PTMService.Common.Entities.TestResult", b =>
                {
                    b.HasOne("Microsoft.Protocols.TestManager.PTMService.Common.Entities.TestSuiteConfiguration", "TestSuiteConfiguration")
                        .WithMany()
                        .HasForeignKey("TestSuiteConfigurationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TestSuiteConfiguration");
                });

            modelBuilder.Entity("Microsoft.Protocols.TestManager.PTMService.Common.Entities.TestSuiteConfiguration", b =>
                {
                    b.HasOne("Microsoft.Protocols.TestManager.PTMService.Common.Entities.TestSuiteInstallation", "TestSuiteInstallation")
                        .WithMany()
                        .HasForeignKey("TestSuiteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TestSuiteInstallation");
                });
#pragma warning restore 612, 618
        }
    }
}