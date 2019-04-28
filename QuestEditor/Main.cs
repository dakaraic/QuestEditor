// Copyright 2019 RED Software, LLC. All Rights Reserved.

using QuestEditor.IO;
using QuestEditor.Quests;
using QuestEditor.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Forms;

namespace QuestEditor
{
    public partial class Main : Form
    {
        public static Quest ActiveQuest;
        public static UI.Quests Quests;

        public static Dictionary<uint, string> QuestDialog;
        public static Dictionary<ushort, Quest> QuestData;

        private ushort header;
        private string filePath;
        private bool hasVerifiedSSL;

        public Main()
        {
            InitializeComponent();
            header = 6;

            massModifiersToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[]
            {
                massEXPModifierToolStripMenuItem,
                massMoneyModifierToolStripMenuItem,
                massFameModifierToolStripMenuItem
            });
        }

        /// <summary>
        /// Finds the MAC address of the NIC with maximum speed.
        /// </summary>
        /// <returns>The MAC address.</returns>
        private string GetMacAddress()
        {
            var macAddress = string.Empty;
            
            foreach (var nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (macAddress == string.Empty)
                {
                    macAddress = nic.GetPhysicalAddress().ToString();
                }
            }

            return macAddress;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Title = "Open QuestData.shn";
                dialog.Filter = "QuestData.shn|QuestData.shn";

                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                filePath = dialog.FileName;
                Open();
            }
        }

        private void Open()
        {
            if (QuestData != null)
            {
                CloseFile();
            }

            ParseQuestDialog(filePath);
            ParseQuestData(filePath);

            Quests = new UI.Quests { Dock = DockStyle.Fill };

            inactiveLogo.Visible = false;
            saveToolStripMenuItem.Enabled = true;
            closeToolStripMenuItem.Enabled = true;
            deleteAllQuestsToolStripMenuItem.Enabled = true;
            newQuestToolStripMenuItem.Enabled = true;
            deleteToolStripMenuItem.Enabled = true;
            massModifiersToolStripMenuItem.Enabled = true;
            massEXPModifierToolStripMenuItem.Enabled = true;
            massMoneyModifierToolStripMenuItem.Enabled = true;
            massFameModifierToolStripMenuItem.Enabled = true;

            mainPanel.Controls.Add(Quests);
            Quests.RefreshList();
        }

        private void ParseQuestDialog(string fileName)
        {
            var path = fileName.Replace("QuestData", "QuestDialog");

            if (!File.Exists(path))
            {
                MessageBox.Show("Failed to open corresponding QuestDialog.shn.\nYou can still edit quests, just without the dialogs.");
                return;
            }

            QuestDialog = new Dictionary<uint, string>();

            using (var file = new SHNFile(path))
            using (var reader = new DataTableReader(file.Table))
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        QuestDialog.Remove((uint) reader.GetValue(0));
                        QuestDialog.Add((uint) reader.GetValue(0), reader.GetString(1));
                    }
                }
            }
        }

        private void ParseQuestData(string fileName)
        {
            QuestData = new Dictionary<ushort, Quest>();

            using (var stream = new MemoryStream(File.ReadAllBytes(fileName)))
            using (var reader = new System.IO.BinaryReader(stream))
            {
                header = reader.ReadUInt16();
                var count = reader.ReadInt16();

                for (var i = 0; i < count; i++)
                {
                    var size = reader.ReadInt32();
                    var data = reader.ReadBytes(size - 4);

                    var parsed = Quest.Parse(data);
                    if (parsed == null)
                    {
                        MessageBox.Show("Failed to parse QuestData.shn");
                        return;
                    }

                    QuestData.Add(parsed.ID, parsed);
                }
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseFile();
        }

        private void CloseFile()
        {
            if (Quests == null)
            {
                return;
            }

            ActiveQuest = null;

            QuestData = null;
            QuestDialog = null;

            saveToolStripMenuItem.Enabled = false;
            closeToolStripMenuItem.Enabled = false;
            deleteAllQuestsToolStripMenuItem.Enabled = false;
            newQuestToolStripMenuItem.Enabled = false;
            deleteToolStripMenuItem.Enabled = false;
            massModifiersToolStripMenuItem.Enabled = false;
            massEXPModifierToolStripMenuItem.Enabled = false;
            massMoneyModifierToolStripMenuItem.Enabled = false;
            massFameModifierToolStripMenuItem.Enabled = false;

            mainPanel.Controls.Remove(Quests);
            inactiveLogo.Visible = true;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Quests == null)
            {
                return;
            }

            SaveFile();
        }

        private void SaveFile()
        {
            Quests?.SaveView(); // Saves the currently active quest.

            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                writer.Write(header);
                writer.Write((ushort) QuestData.Count);

                foreach (var quest in QuestData.Values)
                {
                    using (var questStream = new MemoryStream())
                    using (var questWriter = new BinaryWriter(questStream))
                    {
                        questWriter.Write(quest.ID);
                        questWriter.Fill(2);
                        questWriter.Write(quest.NameID);
                        questWriter.Write(quest.BriefID);
                        questWriter.Write(quest.Region);
                        questWriter.Write((byte) quest.Type);
                        questWriter.Write(quest.IsRepeatable);
                        questWriter.Write((byte) quest.DailyQuestType);
                        questWriter.Fill(4);

                        questWriter.Write(quest.StartCondition.IsWaitListView);
                        questWriter.Write(quest.StartCondition.IsWaitListProgress);
                        questWriter.Write(quest.StartCondition.RequiresLevel);
                        questWriter.Write(quest.StartCondition.MinLevel);
                        questWriter.Write(quest.StartCondition.MaxLevel);
                        questWriter.Write(quest.StartCondition.RequiresNPC);
                        questWriter.Write(quest.StartCondition.NPCID);
                        questWriter.Write(quest.StartCondition.RequiresItem);
                        questWriter.Fill(1);
                        questWriter.Write(quest.StartCondition.ItemID);
                        questWriter.Write(quest.StartCondition.ItemLot);
                        questWriter.Write(quest.StartCondition.RequiresLocation);
                        questWriter.Fill(1);
                        questWriter.Write(quest.StartCondition.LocationMapID);
                        questWriter.Fill(2);
                        questWriter.Write(quest.StartCondition.LocationX);
                        questWriter.Write(quest.StartCondition.LocationY);
                        questWriter.Write(quest.StartCondition.LocationRange);
                        questWriter.Write(quest.StartCondition.RequiresQuest);
                        questWriter.Fill(1);
                        questWriter.Write(quest.StartCondition.QuestID);
                        questWriter.Write(quest.StartCondition.RequiresRace);
                        questWriter.Write(quest.StartCondition.Race);
                        questWriter.Write(quest.StartCondition.RequiresClass);
                        questWriter.Write((byte) quest.StartCondition.Class);
                        questWriter.Write(quest.StartCondition.RequiresGender);
                        questWriter.Write((byte) quest.StartCondition.Gender);
                        questWriter.Write(quest.StartCondition.RequiresDateMode);
                        questWriter.Write((byte) quest.StartCondition.DateMode);
                        questWriter.Fill(4);
                        questWriter.Write(quest.StartCondition.StartDate);
                        questWriter.Write(quest.StartCondition.EndDate);

                        questWriter.Write(quest.EndCondition.IsWaitListProgress);
                        questWriter.Write(quest.EndCondition.RequiresLevel);
                        questWriter.Write(quest.EndCondition.Level);
                        questWriter.Fill(1);

                        for (var i = 0; i < 5; i++)
                        {
                            var mob = quest.EndCondition.NPCMobs[i];

                            questWriter.Write(mob.IsRequired);
                            questWriter.Fill(1);
                            questWriter.Write(mob.ID);
                            questWriter.Write((byte) mob.Action);
                            questWriter.Write(mob.Count);
                            questWriter.Write(mob.TargetGroup);
                            questWriter.Fill(1);
                        }

                        for (var i = 0; i < 5; i++)
                        {
                            var item = quest.EndCondition.Items[i];

                            questWriter.Write(item.IsRequired);
                            questWriter.Fill(1);
                            questWriter.Write(item.ID);
                            questWriter.Write(item.Lot);
                        }

                        questWriter.Write(quest.EndCondition.RequiresLocation);
                        questWriter.Fill(1);
                        questWriter.Write(quest.EndCondition.LocationMapID);
                        questWriter.Fill(2);
                        questWriter.Write(quest.EndCondition.LocationX);
                        questWriter.Write(quest.EndCondition.LocationY);
                        questWriter.Write(quest.EndCondition.LocationRange);
                        questWriter.Write(quest.EndCondition.IsScenario);
                        questWriter.Fill(1);
                        questWriter.Write(quest.EndCondition.ScenarioID);
                        questWriter.Write(quest.EndCondition.RequiresRace);
                        questWriter.Write(quest.EndCondition.Race);
                        questWriter.Write(quest.EndCondition.RequiresClass);
                        questWriter.Write((byte) quest.EndCondition.Class);
                        questWriter.Write(quest.EndCondition.IsTimeLimit);
                        questWriter.Fill(1);
                        questWriter.Write(quest.EndCondition.TimeLimit);

                        questWriter.Write(quest.ActionCount);
                        questWriter.Fill(3);

                        for (var i = 0; i < 10; i++)
                        {
                            var action = quest.Actions[i];

                            questWriter.Write((byte) action.IfType);
                            questWriter.Fill(3);
                            questWriter.Write(action.IfTarget);
                            questWriter.Write((byte) action.ThenType);
                            questWriter.Fill(3);
                            questWriter.Write(action.ThenTarget);
                            questWriter.Write(action.ThenPercent);
                            questWriter.Write(action.ThenMinCount);
                            questWriter.Write(action.ThenMaxCount);
                            questWriter.Write(action.TargetGroup);
                            questWriter.Fill(3);
                        }

                        for (var i = 0; i < 12; i++)
                        {
                            var reward = quest.Rewards[i];

                            questWriter.Write((byte) reward.Use);
                            questWriter.Write((byte) reward.Type);
                            questWriter.Fill(2);

                            switch (reward.Type)
                            {
                                case QuestRewardType.QRT_EXP:
                                    questWriter.Write(reward.Value.EXP);
                                    break;
                                case QuestRewardType.QRT_MONEY:
                                    questWriter.Write(reward.Value.Money);
                                    break;
                                case QuestRewardType.QRT_ITEM:
                                    questWriter.Write(reward.Value.ItemID);
                                    questWriter.Write(reward.Value.ItemLot);
                                    questWriter.Fill(4);
                                    break;
                                case QuestRewardType.QRT_ABSTATE:
                                    questWriter.Write(reward.Value.AbStateKeepTime);
                                    questWriter.Write(reward.Value.AbStateID);
                                    questWriter.Write(reward.Value.AbStateStrength);
                                    questWriter.Fill(1);
                                    break;
                                case QuestRewardType.QRT_FAME:
                                    questWriter.Write(reward.Value.Fame);
                                    questWriter.Fill(4);
                                    break;
                                case QuestRewardType.QRT_PET:
                                    questWriter.Write(reward.Value.PetID);
                                    questWriter.Fill(4);
                                    break;
                                case QuestRewardType.QRT_MINIHOUSE:
                                    questWriter.Write(reward.Value.MiniHouseID);
                                    questWriter.Fill(7);
                                    break;
                                case QuestRewardType.QRT_TITLE:
                                    questWriter.Write(reward.Value.TitleType);
                                    questWriter.Write(reward.Value.TitleElementNo);
                                    questWriter.Fill(6);
                                    break;
                                case QuestRewardType.QRT_KILLPOINT:
                                    questWriter.Write(reward.Value.KillPoints);
                                    questWriter.Fill(4);
                                    break;
                            }
                        }

                        questWriter.Write(quest.ScriptStartSize);
                        questWriter.Write(quest.ScriptDoingSize);
                        questWriter.Write(quest.ScriptEndSize);
                        questWriter.Fill(2);
                        questWriter.Write(quest.StartScriptID);
                        questWriter.Write(quest.DoingScriptID);
                        questWriter.Write(quest.EndScriptID);
                        questWriter.Write(Encoding.ASCII.GetBytes(quest.StartScript));
                        questWriter.Write(Encoding.ASCII.GetBytes(quest.DoingScript));
                        questWriter.Write(Encoding.ASCII.GetBytes(quest.EndScript));

                        var bytes = questStream.ToArray();

                        writer.Write(bytes.Length + 4);
                        writer.Write(bytes);
                    }
                }

                File.WriteAllBytes(filePath, stream.ToArray());
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Exit())
            {
                return;
            }

            Environment.Exit(0);
        }

        private static bool Exit()
        {
            if (QuestData == null)
            {
                return true;
            }

            var result = MessageBox.Show("Are you sure you want to exit?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            return result == DialogResult.Yes;
        }

        private static bool Save()
        {
            var result = MessageBox.Show("Do you want to save the current file?", "Save", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            return result == DialogResult.Yes;
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !Exit();
        }

        private void newQuestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (QuestData == null)
            {
                return;
            }

            var newID = (ushort) (QuestData.Values.OrderBy(q => q.ID).LastOrDefault()?.ID + 1 ?? 1);
            var quest = new Quest {ID = newID, NameID = -1};

            QuestData.Add(newID, quest);

            Quests.RefreshList();
            Quests.Open(quest);
        }

        private void deleteAllQuestsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (QuestData == null)
            {
                return;
            }

            QuestData.Clear();
            ActiveQuest = null;

            Quests.RefreshList();
            Quests.RefreshQuest();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (QuestData != null)
            {
                // Save the current file
                if (Save())
                {
                    SaveFile();
                }
            }

            using (var dialog = new SaveFileDialog())
            {
                dialog.Title = "Choose Location";
                dialog.Filter = "QuestData.shn|*.shn";

                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                filePath = dialog.FileName;

                QuestData = new Dictionary<ushort, Quest>();

                SaveFile();
                Open();
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Quests?.DeleteCurrent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            if (!CheckForInternetConnection())
            {
                MessageBox.Show("You are not connected to the internet.", "RED Software Quest Editor", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }

            try
            {
                ServicePointManager.ServerCertificateValidationCallback += ValidateRemoteCertificate;

                using (var client = new WebClient())
                {
                    var result = client.DownloadString($"https://cdn.redware.co/quest/auth.php?mac={GetMacAddress()}");

                    if (!hasVerifiedSSL || result != "52FWG3DXU0ALPR03N3NK")
                    {
                        MessageBox.Show("You do not have access to this software.\nPurchase the product from RED Software to gain access.", "RED Software Quest Editor", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Environment.Exit(0);
                    }
                }
            }
            catch
            {
                MessageBox.Show("Failed to authenticate the user.", "RED Software Quest Editor", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }
        }

        private bool ValidateRemoteCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslpolicyerrors)
        {
            if (sslpolicyerrors != SslPolicyErrors.None)
            {
                return false;
            }

            if (certificate.GetCertHashString() != "ADDF4825DA5FDF5182DE6AD3ED8D4327244307A9" || certificate.Subject != "CN=sni219021.cloudflaressl.com, OU=PositiveSSL Multi-Domain, OU=Domain Control Validated")
            {
                Environment.Exit(0);
            }

            hasVerifiedSSL = true;
            return true;
        }

        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead("http://clients3.google.com/generate_204"))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        private static readonly int KEY_SIZE = 32;
        private static readonly byte[] IV = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        public static string EncryptString(string plaintext, string password)
        {
            var key = new byte[KEY_SIZE];
            var passwordbytes = Encoding.UTF8.GetBytes(password);

            for (int i = 0; i < KEY_SIZE; i++)
            {
                if (i >= passwordbytes.Length)
                    key[i] = 0;
                else
                    key[i] = passwordbytes[i];
            }

            byte[] encrypted;

            // Create an AesCryptoServiceProvider object
            // with the specified key and IV.
            using (var aesAlg = new AesManaged())
            {
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.KeySize = KEY_SIZE * 8;

                // Create a decrytor to perform the stream transform.
                var encryptor = aesAlg.CreateEncryptor(key, IV);

                // Create the streams used for encryption.
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plaintext);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(encrypted);
        }

        private void MassEXPModifierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new MassEXPEditor();
            form.ShowDialog(this);
        }

        private void MassMoneyModifierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new MassCenEditor();
            form.ShowDialog(this);
        }

        private void MassFameModifierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new MassFameEditor();
            form.ShowDialog(this);
        }
    }
}