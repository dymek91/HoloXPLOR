﻿using NClone;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Diagnostics;
using Inventory = HoloXPLOR.Data.Xml.Inventory;
using Ships = HoloXPLOR.Data.Xml.Vehicles.Implementations;
using Items = HoloXPLOR.Data.Xml.Spaceships;
using Xml = HoloXPLOR.Data.Xml;
using HoloXPLOR.Models;
using HoloXPLOR.DataForge;
using System.Net;
using System.Configuration;
using System.Web.Hosting;

namespace System
{
    public static class __Proxy
    {
        public static String ToLocalized(this String input) { return HoloXPLOR.HoloXPLOR_App.Scripts.Localization.GetValue(input ?? String.Empty, input); }
    }
}

namespace HoloXPLOR.Data
{
    public class Scripts
    {
        private String _scriptRoot;

        public Scripts(String scriptRoot)
        {
            this._scriptRoot = scriptRoot;
        }

        #region Item XML

        private Object _itemsLock = new Object();
        private Dictionary<String, Xml.Spaceships.Item> _items;
        public Dictionary<String, Xml.Spaceships.Item> Items
        {
            get
            {
                if (this._items == null)
                {
                    lock (this._itemsLock)
                    {
                        if (this._items == null)
                        {
                            var buffer = new Dictionary<String, Xml.Spaceships.Item>(StringComparer.InvariantCultureIgnoreCase);

                            DirectoryInfo weaponsDir = new DirectoryInfo(Path.Combine(this._scriptRoot, "Entities/Items/XML/Spaceships"));

                            foreach (FileInfo file in weaponsDir.GetFiles("*.xml", SearchOption.AllDirectories))
                            {
                                if (file.FullName.Contains(@"Interface") ||
                                    file.FullName.Contains(@"\Ammo\Ballistic_Ammo\") ||
                                    file.FullName.Contains(@"\Ammo\Countermeasures\") ||
                                    file.FullName.Contains(@"\Ammo\Laser_Bolts\") ||
                                    file.FullName.Contains(@"\Ammo\Rocket_Ammo\"))
                                    continue;

#if !DEBUG || DEBUG
                                try
                                {
#endif
                                    // var item = CryXmlSerializer.Deserialize<Xml.Spaceships.Item>(file.FullName);
                                    var item = File.ReadAllText(file.FullName).FromXML<Xml.Spaceships.Item>();

                                    if (item == null)
                                        continue;

                                    item = this._CleanEdgeCases(item);

                                    buffer[item.Name] = this._CleanEdgeCases(item);
#if !DEBUG || DEBUG
                                }
                                catch (Exception ex)
                                {
                                    Debug.WriteLine("Unable to parse {0} - {1}", file.FullName, ex.Message);
                                }
#endif
                            }

                            this._items = buffer;
                        }
                    }
                }

                return this._items;
            }
        }

        private Items.Item _CleanEdgeCases(Items.Item item)
        {
            #region Edge Case Support

            if (item.AmmoBox != null && item.AmmoBox.Items != null)
            {
                switch (item.AmmoBox["ammo_name"])
                {
                    case "BEHR_Flare":
                        item.DisplayName = String.Format("{0} Flares", item.AmmoBox["max_ammo_count"]);
                        item.Params["itemSubType"] = "Ammo_CounterMeasure";
                        break;
                    case "TALN_Chaff":
                        item.DisplayName = String.Format("{0} Chaff", item.AmmoBox["max_ammo_count"]);
                        item.Params["itemSubType"] = "Ammo_CounterMeasure";
                        break;
                    case "20mm_AMMO": item.DisplayName = String.Format("{0} 20mm Shells", item.AmmoBox["max_ammo_count"]); break;
                    case "24mm_AMMO": item.DisplayName = String.Format("{0} 24mm Shells", item.AmmoBox["max_ammo_count"]); break;
                    case "25mm_AMMO": item.DisplayName = String.Format("{0} 25mm Shells", item.AmmoBox["max_ammo_count"]); break;
                    case "30mm_AMMO": item.DisplayName = String.Format("{0} 30mm Shells", item.AmmoBox["max_ammo_count"]); break;
                    case "35mm_AMMO": item.DisplayName = String.Format("{0} 35mm Shells", item.AmmoBox["max_ammo_count"]); break;
                    case "40mm_AMMO": item.DisplayName = String.Format("{0} 40mm Shells", item.AmmoBox["max_ammo_count"]); break;
                    case "40mm_5km_AMMO": item.DisplayName = String.Format("{0} 40mm Explosive Shells", item.AmmoBox["max_ammo_count"]); break;
                    case "40mm_5km_exp_AMMO": item.DisplayName = String.Format("{0} 40mm Explosive Shells (5km)", item.AmmoBox["max_ammo_count"]); break;
                    case "50mm_AMMO": item.DisplayName = String.Format("{0} 50mm Shells", item.AmmoBox["max_ammo_count"]); break;
                    case "60mm_AMMO": item.DisplayName = String.Format("{0} 60mm Shells", item.AmmoBox["max_ammo_count"]); break;
                    case "60mm_Rail_AMMO": item.DisplayName = String.Format("{0} 60mm Slugs", item.AmmoBox["max_ammo_count"]); break;
                    case "80mm_AMMO": item.DisplayName = String.Format("{0} 80mm Shells", item.AmmoBox["max_ammo_count"]); break;
                    case "80mm_Rail_AMMO": item.DisplayName = String.Format("{0} 60mm Slugs", item.AmmoBox["max_ammo_count"]); break;
                    case "106mm_exp_AMMO": item.DisplayName = String.Format("{0} 106mm Explosive Shells", item.AmmoBox["max_ammo_count"]); break;
                    case "Rocket_AMMO": item.DisplayName = String.Format("{0} Delta Rockets", item.AmmoBox["max_ammo_count"]); break;
                    default: item.DisplayName = String.Format("{0} {1}", item.AmmoBox["max_ammo_count"], item.AmmoBox["ammo_name"].Replace("_", " ")); break;
                }
            }

            if (item.Ports != null && item.Ports.Items != null)
            {
                var cmp = item.Ports.Items.Where(p => p.Types != null).SelectMany(p => p.Types.Where(t => t.Type == "AmmoBox").Where(t => String.IsNullOrWhiteSpace(t.SubType)));

                foreach (var port in cmp)
                {
                    port.SubType = "Ammo_CounterMeasure";
                }
            }

            switch (item.Name)
            {
                case "GODI_Retaliator_Shield_S3": item.Params["requiredPortTags"] = "AEGS_Retaliator_Base"; break;
                case "Class_2_KRIG_BG_S3_Q3_Mount": item.DisplayName = "Kruger S3 Gattling Nose Mount"; break;
                case "Class_2_GATS_BG_S2_Mount": item.DisplayName = "Gallenson Tactical S2 Gattling Mount"; break;
                case "BEHR_PC2_Dual_S3": item.DisplayName = "Behring Dual Side Turret"; break;
                case "ANVL_Gladiator_Turret_Ball_S2_Q2": item.DisplayName = "Gladiator S4 Ball Turret"; break;
                case "DRAK_Cutlass_Turret": item.DisplayName = "Cutlass Manned Turret"; break;
                case "MISC_Freelancer_Turret": item.DisplayName = "Freelancer Manned Turret"; break;
                case "AEGS_Vanguard_Turret": item.DisplayName = "Vanguard Manned Turret"; break;
                case "AEGS_Retaliator_Turret": item.DisplayName = "Retaliator Manned Turret"; break;
                case "BRRA_HornetCanard_S2_Q1": item.DisplayName = "Hornet S3 Canard Mount"; break;
                case "BRRA_HornetBall_S2_Q1": item.DisplayName = "Hornet S5 Ball Turret"; break;
                case "CNOU_Mustang_S1_Q2": item.DisplayName = "Consolidated Outland Ball Turret"; break;
                // case "BRRA_HornetBall_160f_S1_Q2": item.DisplayName = "Hornet S5 Ball Turret"; break;
                case "ANVL_Fixed_Mount_Hornet_Ball_S4": item.DisplayName = "Hornet S4 Fixed Mount"; break;
                case "DRAK_Fixed_Mount_S4": item.DisplayName = "Cutlass S4 Fixed Mount"; break;
                case "BEHR_PC2_Dual_S4_Fixed": item.DisplayName = "Behring S4 Twin Rack"; break;
                case "AEGS_S2_Rack_x4": item.DisplayName = "Aegis S2 Quad Rack"; break;
                case "ANVL_S5_Rack_x2": item.DisplayName = "Anvil S5 Twin Rack"; break;
                case "Talon_Stalker_Twin": item.DisplayName = "Talon Stalker S2 Twin Rack"; break;
                case "Talon_Stalker_Quad": item.DisplayName = "Talon Stalker S1 Quad Rack"; break;
                case "Talon_Stalker_Platform_x2": item.DisplayName = "Talon Stalker S2 Twin Rack"; break;
                case "Talon_Stalker_Platform_x4": item.DisplayName = "Talon Stalker S1 Quad Rack"; break;
                case "VNCL_Mark_Platform_x4": item.DisplayName = "Vanduul S1 Quad Rack"; break;

                case "Mount_Gimbal_S1": item.DisplayName = "S1 Gimbal Mount"; break;
                case "Mount_Gimbal_S2": item.DisplayName = "S2 Gimbal Mount"; break;
                case "Mount_Gimbal_S3": item.DisplayName = "S3 Gimbal Mount"; break;
                case "Mount_Gimbal_S4": item.DisplayName = "S4 Gimbal Mount"; break;
                case "Mount_Gimbal_S5": item.DisplayName = "S5 Gimbal Mount"; break;

                case "JOKR_DistortionCannon_S1": item.DisplayName = "\"Suckerpunch\" Distortion Cannon"; break;
                case "VNCL_Weak_LC_S1": item.DisplayName = "'Weak' Laser Cannon"; break;
                case "KBAR_BallisticCannon_S1": item.DisplayName = "9-Series Longsword"; break;
                case "KBAR_BallisticCannon_S3": item.DisplayName = "11-Series Broadsword"; break;
                case "GATS_BallisticGatling_S2": item.DisplayName = "Scorpion GT-215"; break;
                case "GATS_BallisticGatling_S3": item.DisplayName = "Mantis GT-220"; break;
                case "GATS_BallisticCannon_S2": item.DisplayName = "Tarantula GT-870"; break;
                case "GATS_BallisticCannon_S3": item.DisplayName = "Tarantula GT-870 Mark 3"; break;
                case "KLWE_LaserRepeater_S3": item.DisplayName = "CF-227 Panther Repeater"; break;
                case "KLWE_LaserRepeater_S2": item.DisplayName = "CF-117 Badger Repeater"; break;
                case "KLWE_LaserRepeater_S1": item.DisplayName = "CF-007 Bulldog Repeater"; break;
                case "KLWE_MassDriverCannon_S1": item.DisplayName = "Sledge II Mass Driver Cannon"; break;
                case "KRIG_BallisticGatling_S3": item.DisplayName = "Kruger Tigerstreik T-21"; break;
                case "KRIG_BallisticGatling_S2_Parasite": item.DisplayName = "Kruger Tigerstreik T-19P"; break;
                case "APAR_MassDriver_S2": item.DisplayName = "Strife Mass Driver"; break;
                case "APAR_BallisticGatling_S4": item.DisplayName = "Death Ballistic Gattling"; break;
                case "AEGS_EMP_Device": item.DisplayName = "REP-8 EMP Generator"; break;
                case "BEHR_LaserCannon_S1": item.DisplayName = "M3A Laser Cannon"; break;
                case "BEHR_LaserCannon_S2": item.DisplayName = "M4A Laser Cannon"; break;
                case "BEHR_LaserCannon_Vanguard_S2": item.DisplayName = "M4A Laser Cannon - Vanguard"; break;
                case "BEHR_LaserCannon_S3": item.DisplayName = "M5A Laser Cannon"; break;
                case "BEHR_LaserCannon_S4": item.DisplayName = "M6A Laser Cannon"; break;
                case "BEHR_BallisticCannon_S4": item.DisplayName = "C-788 \"Combine\" Ballistic Cannon"; break;
                case "BEHR_BallisticRepeater_S2": item.DisplayName = "SW16BR2 \"Sawbuck\""; break;
                case "MXOX_NeutronCannon_S1": item.DisplayName = "Maxox NN-13 Neutron Cannon"; break;
                case "MXOX_NeutronCannon_S2": item.DisplayName = "Maxox NN-14 Neutron Cannon"; break;
                case "AMRS_LaserCannon_S1": item.DisplayName = "Omnisky III Laser Cannon"; break;
                case "AMRS_LaserCannon_S2": item.DisplayName = "Omnisky VI Laser Cannon"; break;
                case "CNOU_Delta_RocketPod_x18": item.DisplayName = "R-18 rocket pod"; break;

                case "VNCL_MissileRack_Blade": item.DisplayName = "Vanduul Blade"; break;
                case "RSI_Constellation_Turret": item.DisplayName = "Constellation Turret"; break;
                case "RSI_Constellation_MissilePod_S2_x3": item.DisplayName = "Constellation S2 Triple Rack"; break;
                case "RSI_Constellation_MissilePod_S1_x7": item.DisplayName = "Constellation S1 Heavy Rack"; break;
            }

            #endregion

            return item;
        }

        #endregion

        #region Ammo XML

        private Object _ammoLock = new Object();
        private Dictionary<String, DataForge.AmmoParams> _ammo;
        public Dictionary<String, DataForge.AmmoParams> Ammo
        {
            get
            {
                if (this._ammo == null)
                {
                    lock (this._ammoLock)
                    {
                        if (this._ammo == null)
                        {
                            var buffer = new Dictionary<String, DataForge.AmmoParams>(StringComparer.InvariantCultureIgnoreCase);

                            DirectoryInfo ammoDir = new DirectoryInfo(Path.Combine(this._scriptRoot, "Libs/Foundry/Records/AmmoParams/Vehicle"));

                            foreach (FileInfo file in ammoDir.GetFiles("*.xml", SearchOption.AllDirectories))
                            {
#if !DEBUG || DEBUG
                                try
                                {
#endif
                                    // var ammo = CryXmlSerializer.Deserialize<Xml.Spaceships.Ammo>(file.FullName);
                                    var ammo = File.ReadAllText(file.FullName).FromXML<DataForge.AmmoParams>();

                                    if (ammo == null)
                                        continue;

                                    buffer[Path.GetFileNameWithoutExtension(file.Name).Replace("AmmoParams.", "")] = this._CleanEdgeCases(ammo);
#if !DEBUG || DEBUG
                                }
                                catch (Exception ex)
                                {
                                    Debug.WriteLine("Unable to parse {0} - {1}", file.FullName, ex.Message);
                                }
#endif
                            }

                            this._ammo = buffer;
                        }
                    }
                }

                return this._ammo;
            }
        }

        private DataForge.AmmoParams _CleanEdgeCases(DataForge.AmmoParams ammo)
        {
            #region Edge Case Support

            #endregion

            return ammo;
        }

        #endregion

        #region Loadout XML

        private Object _loadoutLock = new Object();
        private Dictionary<String, Xml.Spaceships.Loadout> _loadout;
        public Dictionary<String, Xml.Spaceships.Loadout> Loadout
        {
            get
            {
                if (this._loadout == null)
                {
                    lock (this._loadoutLock)
                    {
                        if (this._loadout == null)
                        {
                            var buffer = new Dictionary<String, Xml.Spaceships.Loadout>(StringComparer.InvariantCultureIgnoreCase);

                            DirectoryInfo loadoutDir = new DirectoryInfo(Path.Combine(this._scriptRoot, "Loadouts/Vehicles"));

                            foreach (FileInfo file in loadoutDir.GetFiles("*.xml", SearchOption.AllDirectories))
                            {
                                if (file.FullName.Contains(@"Interface"))
                                    continue;
#if !DEBUG || DEBUG
                                try
                                {
#endif
                                    // var loadout = CryXmlSerializer.Deserialize<Xml.Spaceships.Loadout>(file.FullName);
                                    var loadout = File.ReadAllText(file.FullName).FromXML<Xml.Spaceships.Loadout>();

                                    if (loadout == null)
                                        continue;

                                    String name = Path.GetFileNameWithoutExtension(file.Name).Replace("Default_Loadout_", "");

                                    buffer[name] = loadout;
#if !DEBUG || DEBUG
                                }
                                catch (Exception ex)
                                {
                                    Debug.WriteLine("Unable to parse {0} - {1}", file.FullName, ex.Message);
                                }
#endif
                            }

                            this._loadout = buffer;
                        }
                    }
                }

                return this._loadout;
            }
        }

        #endregion

        #region Vehicle XML

        private Object _vehicleLock = new Object();
        private Dictionary<String, Ships.Vehicle> _vehicles;
        public Dictionary<String, Ships.Vehicle> Vehicles
        {
            get
            {
                if (this._vehicles == null)
                {
                    lock (this._vehicleLock)
                    {
                        if (this._vehicles == null)
                        {
                            var buffer = new Dictionary<String, Ships.Vehicle>(StringComparer.InvariantCultureIgnoreCase);

                            DirectoryInfo vehicleDir = new DirectoryInfo(Path.Combine(this._scriptRoot, "Entities/Vehicles/Implementations/Xml"));

                            #region Load all vehicle files

                            foreach (FileInfo file in vehicleDir.GetFiles("*.xml", SearchOption.TopDirectoryOnly))
                            {
#if !DEBUG
                        try
                        {
#endif
                                // var vehicle = CryXmlSerializer.Deserialize<Ships.Vehicle>(file.FullName);
                                var vehicle = File.ReadAllText(file.FullName).FromXML<Ships.Vehicle>();

                                if (vehicle == null)
                                    continue;

                                vehicle = this._CleanEdgeCases(vehicle);

                                buffer[vehicle.Name] = vehicle;

                                if (vehicle.Name == "CNOU_Mustang")
                                    buffer["CNOU_Mustang_Alpha"] = vehicle;

                                if (vehicle.Name == "ORIG")
                                    buffer["ORIG_300i"] = vehicle;

                                if (vehicle.Name == "AEGS_Avenger")
                                    buffer["AEGS_Avenger_Stalker"] = vehicle;

                                // if (vehicle.Name == "AEGS_Avenger_Stalker_Warlock")
                                //     buffer["AEGS_Avenger_Warlock"] = vehicle;
                                // if (vehicle.Name == "AEGS_Avenger_Stalker_Titan")
                                //     buffer["AEGS_Avenger_Titan"] = vehicle;

                                #region Variant Support

                                if (vehicle.Modifications != null && vehicle.Modifications.Length > 0)
                                {
                                    foreach (var modification in vehicle.Modifications)
                                    {
                                        var variantXML = new XmlDocument();
                                        variantXML.LoadXml(vehicle.ToXML().Remove(0, 39));

                                        var replacementParts = modification.Parts;

                                        #region Patch Support

                                        if (!String.IsNullOrWhiteSpace(modification.PatchFile))
                                        {
                                            // var patch = CryXmlSerializer.Deserialize<Ships.Modification>(HostingEnvironment.MapPath(String.Format(@"~/App_Data/Scripts/Entities/Vehicles/Implementations/Xml/{0}.xml", modification.PatchFile)));
                                            var patch = File.ReadAllText(Path.Combine(this._scriptRoot, String.Format("Entities/Vehicles/Implementations/Xml/{0}.xml", modification.PatchFile))).FromXML<Ships.Modification>();

                                            if (patch.Parts != null && patch.Parts.Length > 0)
                                            {
                                                replacementParts = patch.Parts;
                                            }

                                            patch.Elements = patch.Elements ?? new Ships.Element[] { };

                                            foreach (var difference in patch.Elements)
                                            {
                                                var element = variantXML.SelectSingleNode(String.Format("//*[@id='{0}']", difference.IDRef));

                                                if (element != null)
                                                {
                                                    var attribute = element.Attributes[difference.Name];
                                                    if (attribute == null)
                                                    {
                                                        attribute = variantXML.CreateAttribute(difference.Name);
                                                        element.Attributes.Append(attribute);
                                                    }
                                                    attribute.Value = difference.Value;
                                                }
                                            }
                                        }

                                        #endregion

                                        modification.Elements = modification.Elements ?? new Ships.Element[] { };

                                        foreach (var difference in modification.Elements)
                                        {
                                            var element = variantXML.SelectSingleNode(String.Format("//*[@id='{0}']", difference.IDRef));

                                            if (element != null)
                                            {
                                                var attribute = element.Attributes[difference.Name];
                                                if (attribute == null)
                                                {
                                                    attribute = variantXML.CreateAttribute(difference.Name);
                                                    element.Attributes.Append(attribute);
                                                }
                                                attribute.Value = difference.Value;
                                            }
                                        }

                                        var variant = variantXML.InnerXml.FromXML<Xml.Vehicles.Implementations.Vehicle>();

                                        if (replacementParts != null && replacementParts.Length > 0)
                                            variant.Parts = replacementParts;

                                        if (vehicle.Name.Split('_')[0] == modification.Name.Split('_')[0])
                                            variant.Name = modification.Name;
                                        else
                                            variant.Name = String.Format("{0}_{1}", variant.Name, modification.Name);

                                        variant = this._CleanEdgeCases(variant);

                                        variant.Modifications = null;

                                        buffer[variant.Name] = variant;
                                    }
                                }

                                #endregion
#if !DEBUG
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine("Unable to parse {0} - {1}", file.FullName, ex.Message);
                        }
#endif
                            }

                            #endregion

                            this._vehicles = buffer;
                        }
                    }
                }

                return this._vehicles;
            }
        }


        private Ships.Vehicle _CleanEdgeCases(Xml.Vehicles.Implementations.Vehicle vehicle)
        {
            #region Edge Case Support

            switch (vehicle.Name)
            {
                case "GRIN_PTV":
                    vehicle.DisplayName = "Greycat Industries PTV";
                    break;
                case "RSI_Constellation":
                    vehicle.Name = "RSI_Constellation_Andromeda";
                    vehicle.DisplayName = "RSI Constellation Andromeda";
                    break;
                case "ORIG_300i":
                    vehicle.Name = "ORIG";
                    break;
                case "VNCL_Scythe":
                    vehicle.DisplayName = "Vanduul Scythe";
                    break;
                case "VNCL_Glaive_Glaive_Swarm":
                    vehicle.DisplayName = "Vanduul Glaive";
                    vehicle.Name = "VNCL_Glaive";
                    break;
                case "VNCL_Glaive_Glaive_Tutorial":
                    vehicle.DisplayName = "Vanduul Glaive";
                    vehicle.Name = "VNCL_Glaive_Tutorial";
                    break;
                case "AEGS_Avenger_Titan":
                case "AEGS_Avenger_Stalker_Titan":
                    vehicle.Name = "AEGS_Avenger_Titan";
                    vehicle.DisplayName = "Aegis Avenger Titan";
                    break;
                case "AEGS_Avenger_Warlock":
                case "AEGS_Avenger_Stalker_Warlock":
                    vehicle.Name = "AEGS_Avenger_Warlock";
                    vehicle.DisplayName = "Aegis Avenger Warlock";
                    break;
                case "AEGS_Avenger_Stalker":
                    vehicle.Name = "AEGS_Avenger";
                    vehicle.DisplayName = "Aegis Avenger Stalker";
                    break;
                case "AEGS_Retaliator":
                    vehicle.DisplayName = "Aegis Retaliator";
                    break;
                case "AEGS_Gladius":
                    vehicle.DisplayName = "Aegis Gladius";
                    break;
                case "AEGS_Gladiator":
                    vehicle.DisplayName = "Aegis Gladiator";
                    break;
                case "AEGS_Redeemer":
                    vehicle.DisplayName = "Aegis Redeemer";
                    break;
                default:
                    if (String.IsNullOrWhiteSpace(vehicle.DisplayName))
                        vehicle.DisplayName = vehicle.Name;
                    break;
            }

            #endregion

            return vehicle;
        }

        private Ships.Part _GetPartByID(Xml.Vehicles.Implementations.Part[] parts, String id)
        {
            if (parts != null && parts.Length > 0)
            {
                foreach (var part in parts)
                {
                    if (part.ID == id)
                        return part;

                    var buffer = this._GetPartByID(part.Parts, id);

                    if (buffer != null)
                        return buffer;
                }
            }

            return null;
        }

        #endregion

        private Dictionary<String, String> _localization;
        public Dictionary<String, String> Localization
        {
            get
            {
                if (this._localization == null)
                {
                    this._localization = new Dictionary<String, String>(StringComparer.InvariantCultureIgnoreCase)
                    {
                        #region Turret Port Names

                        { "Countermeasure Launcher", "CounterMeasure Launcher" },
                        { "turrethelper", "Manned Turret" },
                        { "RSI_Constellation_Turret_Base", "Constellation Turret"},
                        { "MISC_Freelancer_Turret_Base", "Freelancer Turret"},
                        { "item_Descdrak_cutlass_s1_q2", "Cutlass Turret" },

                        { "turret_left", "Left Turret Slot"},
                        { "turret_right", "Right Turret Slot"},

                        { "hardpoint_class_1_left", "Left Turret Slot"},
                        { "hardpoint_class_1_right", "Right Turret Slot"},

                        { "hardpoint_turret_backtop", "Back Top Turret" },
                        { "hardpoint_turret_backtopleft", "Back Top Left Turret" },
                        { "hardpoint_turret_backtopright", "Back Top Right Turret" },
                        { "hardpoint_turret_backbottom", "Back Bottom Turret" },
                        { "hardpoint_turret_fronttop", "Front Top Turret" },
                        { "hardpoint_turret_fronttopleft", "Front Top Left Turret" },
                        { "hardpoint_turret_fronttopright", "Front Top Right Turret" },
                        { "hardpoint_turret_frontbottom", "Front Bottom Turret" },
                        
                        { "right wing Class 1 Slot", "Right Wing Class 1 Slot" },

                        #endregion

                        #region Shield Port Names

                        { "hardpoint_shield_generator", "Shield Generator" },
                        { "hardpoint_shield_generator_01", "Shield Generator" },
                        { "hardpoint_shield_generator_02", "Shield Generator" },
                        { "hardpoint_shield_generator_03", "Shield Generator" },
                        { "hardpoint_shieldgenerator_left", "Left Shield" },
                        { "hardpoint_shieldgenerator_right", "Right Shield" },

                        #endregion

                        #region Power Plant Port Names

                        { "hardpoint_powerplant", "Power Plant" },
                        { "hardpoint_powerplant_left", "Left Power Plant" },
                        { "hardpoint_powerplant_Right", "Right Power Plant" },
                        { "hardpoint_power_plant_attach", "Power Plant" },
                        { "hardpoint_power_plant_attach_01", "Power Plant" },
                        { "hardpoint_power_plant_attach_02", "Power Plant" },

                        #endregion

                        #region Thruster Port Names

                        { "hardpoint_engine_left_attach", "Main Thruster Left" },
                        { "hardpoint_engine_right_attach", "Main Thruster Right" },
                        { "hardpoint_engine_mid_attach", "Main Thruster" },

                        { "hardpoint_thruster_front_left_bottom", "Lower Front Left Thruster" },
                        { "hardpoint_thruster_front_right_bottom", "Lower Front Right Thruster" },
                        { "hardpoint_thruster_rear_left_bottom", "Lower Rear Left Thruster" },
                        { "hardpoint_thruster_rear_right_bottom", "Lower Rear Right Thruster" },
                        { "hardpoint_thruster_front_left_top", "Upper Front Left Thruster" },
                        { "hardpoint_thruster_front_right_top", "Upper Front Right Thruster" },
                        { "hardpoint_thruster_rear_left_top", "Upper Rear Left Thruster" },
                        { "hardpoint_thruster_rear_right_top", "Upper Rear Right Thruster" },
                        { "hardpoint_thruster_front_left_mid", "Front Left Thruster" },
                        { "hardpoint_thruster_front_right_mid", "Front Right Thruster" },
                        { "hardpoint_thruster_rear_left_mid", "Rear Left Thruster" },
                        { "hardpoint_thruster_rear_right_mid", "Rear Right Thruster" },

                        { "hardpoint_thruster_mid_front_bottom", "Lower Front Thruster" },
                        { "hardpoint_thruster_mid_back_bottom", "Lower Rear Thruster" },
                        { "hardpoint_thruster_mid_front_top", "Upper Front Thruster" },
                        { "hardpoint_thruster_mid_back_top", "Upper Rear Thruster" },
                        { "hardpoint_thruster_mid_front_mid", "Front Thruster" },
                        { "hardpoint_thruster_mid_back_mid", "Rear Thruster" },
                        { "hardpoint_thruster_left_front_bottom", "Lower Front Left Thruster" },
                        { "hardpoint_thruster_left_back_bottom", "Lower Rear Left Thruster" },
                        { "hardpoint_thruster_left_front_top", "Upper Front Left Thruster" },
                        { "hardpoint_thruster_left_back_top", "Upper Rear Left Thruster" },
                        { "hardpoint_thruster_left_front_mid", "Front Left Thruster" },
                        { "hardpoint_thruster_left_back_mid", "Rear Left Thruster" },
                        { "hardpoint_thruster_right_front_bottom", "Lower Front Right Thruster" },
                        { "hardpoint_thruster_right_back_bottom", "Lower Rear Right Thruster" },
                        { "hardpoint_thruster_right_front_top", "Upper Front Right Thruster" },
                        { "hardpoint_thruster_right_back_top", "Upper Rear Right Thruster" },
                        { "hardpoint_thruster_right_front_mid", "Front Right Thruster" },
                        { "hardpoint_thruster_right_back_mid", "Rear Right Thruster" },
                        
                        { "hardpoint_thruster_front_left_lower", "Lower Front Left Thruster" },
                        { "hardpoint_thruster_front_right_lower", "Lower Front Right Thruster" },
                        { "hardpoint_thruster_rear_left_lower", "Lower Rear Left Thruster" },
                        { "hardpoint_thruster_rear_right_lower", "Lower Rear Right Thruster" },
                        { "hardpoint_thruster_front_left_upper", "Upper Front Left Thruster" },
                        { "hardpoint_thruster_front_right_upper", "Upper Front Right Thruster" },
                        { "hardpoint_thruster_rear_left_upper", "Upper Rear Left Thruster" },
                        { "hardpoint_thruster_rear_right_upper", "Upper Rear Right Thruster" },

                        { "hardpoint_engine_left", "Main Thruster Left" },
                        { "hardpoint_engine_right", "Main Thruster Right" },
                        { "hardpoint_thruster_retro_left", "Left Retro Thruster" },
                        { "hardpoint_thruster_retro_right", "Right Retro Thruster" },
                        { "hardpoint_thruster_bottomFL", "Lower Front Left Thruster" },
                        { "hardpoint_thruster_bottomFR", "Lower Front Right Thruster" },
                        { "hardpoint_thruster_bottomRL", "Lower Rear Left Thruster" },
                        { "hardpoint_thruster_bottomRR", "Lower Rear Right Thruster" },
                        { "hardpoint_thruster_topFL", "Upper Front Left Thruster" },
                        { "hardpoint_thruster_topFR", "Upper Front Right Thruster" },
                        { "hardpoint_thruster_topRL", "Upper Rear Left Thruster" },
                        { "hardpoint_thruster_topRR", "Upper Rear Right Thruster" },
                        
                        { "hardpoint_thruster_intake_left_retro", "Left Intake Retro Thruster" },
                        { "hardpoint_thruster_intake_right_retro", "Right Intake Retro Thruster" },

                        { "hardpoint_thruster_front_left_side", "Front Left Side Thruster" },
                        { "hardpoint_thruster_front_right_side", "Front Right Side Thruster" },
                        { "hardpoint_thruster_rear_left_side", "Rear Left Side Thruster" },
                        { "hardpoint_thruster_rear_right_side", "Rear Right Side Thruster" },
                        { "hardpoint_thruster_wing_left_bottom", "Lower Wing Left Thruster" },
                        { "hardpoint_thruster_wing_right_bottom", "Lower Wing Right Thruster" },
                        { "hardpoint_thruster_wing_left_top", "Upper Wing Left Thruster" },
                        { "hardpoint_thruster_wing_right_top", "Upper Wing Right Thruster" },

                        { "hardpoint_thruster_left_retro", "Left Retro Thruster" },
                        { "hardpoint_thruster_right_retro", "Right Retro Thruster" },

                        { "hardpoint_thruster_engine", "Main Thruster" },
                        { "hardpoint_thruster_retro_bottom_left", "Lower Left Retro Thruster" },
                        { "hardpoint_thruster_retro_bottom_right", "Lower Right Retro Thruster" },
                        { "hardpoint_thruster_retro_top_left", "Upper Left Retro Thruster" },
                        { "hardpoint_thruster_retro_top_right", "Upper Right Retro Thruster" },

                        { "hardpoint_engine_attach", "Main Thruster" },
                        { "hardpoint_thruster_left_lower_front", "Lower Front Left Thruster" },
                        { "hardpoint_thruster_right_lower_front", "Lower Front Right Thruster" },
                        { "hardpoint_thruster_left_lower_rear", "Lower Rear Left Thruster" },
                        { "hardpoint_thruster_right_lower_rear", "Lower Rear Right Thruster" },
                        { "hardpoint_thruster_left_upper_front", "Upper Front Left Thruster" },
                        { "hardpoint_thruster_right_upper_front", "Upper Front Right Thruster" },
                        { "hardpoint_thruster_left_upper_rear", "Upper Rear Left Thruster" },
                        { "hardpoint_thruster_right_upper_rear", "Upper Rear Right Thruster" },

                        #endregion
                    };
                }

                return this._localization;
            }
        }

        private Dictionary<Int32, ShipMatrixJson> _shipJsonMap;
        public Dictionary<Int32, ShipMatrixJson> ShipJsonMap
        {
            get { return _shipJsonMap = _shipJsonMap ?? File.ReadAllText(HostingEnvironment.MapPath(@"~/App_Data/shipmatrix.json")).FromJSON<ShipMatrixJson[]>().ToDictionary(k => k.ID, v => v); }
        }

        public Dictionary<String, Int32> ShipJsonLookup = new Dictionary<String, Int32>(StringComparer.InvariantCultureIgnoreCase)
        {
            { "ANVL_Hornet_F7CR", 14 },
            { "ANVL_Hornet_F7CS", 13 },
            { "ANVL_Hornet_F7CM", 15 },
            { "ANVL_Hornet_F7C", 11 },
            { "ANVL_Hornet_F7A", 37 },
            { "RSI_Aurora_CL", 5 },
            { "RSI_Aurora_ES", 1 },
            { "RSI_Aurora_LN", 6 },
            { "RSI_Aurora_LX", 3 },
            { "RSI_Aurora_MR", 4 },
            { "AEGS_Avenger", 100 },
            { "AEGS_Avenger_Stalker", 100 },
            { "AEGS_Avenger_Titan", 102 },
            { "AEGS_Avenger_Warlock", 101 },
            { "ANVL_Gladiator", 64 },
            { "AEGS_Gladius", 60 },
            { "ORIG", 7 },
            { "ORIG_300i", 7 },
            { "ORIG_315p", 8 },
            { "ORIG_325a", 9 },
            { "ORIG_350R", 10 },
            { "MISC_Starfarer", 88 },
            { "MISC_Starfarer_Gemini", 89 },
            { "RSI_Constellation", 45 },
            { "RSI_Constellation_Andromeda", 45 },
            { "RSI_Constellation_Taurus", 46 },
            { "RSI_Constellation_Hangar_Taurus", 46 },
            { "RSI_Constellation_Phoenix", 49 },
            { "RSI_Constellation_Hangar_Phoenix", 49 },
            { "RSI_Constellation_Aquila", 47 },
            { "RSI_Constellation_Hangar_Aquila", 47 },
            { "DRAK_Cutlass", 56 },
            { "DRAK_Cutlass_Black", 56 },
            { "DRAK_Cutlass_Red", 57 },
            { "DRAK_Cutlass_Blue", 58 },
            { "DRAK_Caterpillar", 24 },
            { "VNCL_Glaive", 93 },
            { "VNCL_Scythe", 26 },
            { "AEGS_Vanguard", 75 },
            { "AEGS_Vanguard_Warden", 75 },
            { "AEGS_Vanguard_Harbringer", 95 },
            { "AEGS_Vanguard_Sentinel", 96 },
            { "AEGS_Idris", 27 },
            { "AEGS_Idris_M", 27 },
            { "AEGS_Idris_P", 28 },
            { "AEGS_Retaliator", 72 },
            { "AEGS_Retaliator_Bomber", 72 },
            { "AEGS_Retaliator_Base", 99 },
            { "MISC_Freelancer", 16 },
            { "MISC_Freelancer_Base", 16 },
            { "MISC_Freelancer_DUR", 31 },
            { "MISC_Freelancer_MAX", 32 },
            { "MISC_Freelancer_MIS", 33 },
            { "ORIG_m50", 22 },
            { "CNOU_Mustang", 65 },
            { "CNOU_Mustang_Alpha", 65 },
            { "CNOU_Mustang_Beta", 66 },
            { "CNOU_Mustang_Delta", 69 },
            { "CNOU_Mustang_Gamma", 67 },
            { "CNOU_Mustang_Omega", 70 },
            { "Xian_Khartu", 35 },
            { "Xian_Khartu_Al", 35 },
            { "Xian_Scout", 35 },
            { "Banu_Merchantman", 36 },
            { "ORIG_890_Jump", 55 },
            { "KRIG_P52_Merlin", 92 },
            { "KRIG_P72_Archimedes", 104 },
            { "AEGS_Redeemer", 59 },
            { "AEGS_Sabre", 98 },
        };

        // https://robertsspaceindustries.com/media/kksqne0o8pi8tr/heap_infobox/BroadSword.jpg
        // https://robertsspaceindustries.com/media/kksqne0o8pi8tr/store_small/BroadSword.jpg
        // https://robertsspaceindustries.com/media/kksqne0o8pi8tr/store_slideshow_small/BroadSword.jpg
    }
}