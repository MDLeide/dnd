using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using Cashew.Data;
using DND.Data.DataAccess.Wrappers;
using DND.DataAccess;
using DND.Domain;

namespace DND.Data.DataAccess
{
    public class HardLinkDataAccess : DataAccess<HardLink>, IHardLinkDataAccess
    {
        public HardLinkDataAccess(ICardDataAccess cardDataAccess)
        {
            CardDataAccess = cardDataAccess;
        }


        public ICardDataAccess CardDataAccess { get; set; }

        protected override string InsertStatement => Statements.HardLinks.Insert;
        protected override string UpdateStatement => Statements.HardLinks.Update;
        protected override string DeleteStatement => Statements.HardLinks.Delete;
        protected override string GetByIdStatement => Statements.HardLinks.SelectById;
        protected override string GetAllStatement => null;


        public IEnumerable<HardLink> GetHardLinksByCardId(int id)
        {
            var cmd = GetCommand();
            cmd.Parameters.AddWithValue("@id", id);
            cmd.CommandText = Statements.HardLinks.SelectByOriginId;
            return ExecuteSelectCommand(cmd, Parse, true);
        }

        public IEnumerable<HardLink> GetHardLinksByOriginId(int id)
        {
            var cmd = GetCommand();
            cmd.CommandText = Statements.HardLinks.SelectByOriginId;
            cmd.Parameters.AddWithValue("@id", id);
            return ExecuteSelectCommand(cmd, Parse, true);
        }

        public IEnumerable<HardLink> GetHardLinksByTargetId(int id)
        {
            var cmd = GetCommand();
            cmd.CommandText = Statements.HardLinks.SelectByTargetId;
            cmd.Parameters.AddWithValue("@id", id);
            return ExecuteSelectCommand(cmd, Parse, true);
        }

        protected override void ValidateSave(HardLink link)
        {
            if (link.Target?.Id == null)
                throw new InvalidOperationException("Cannot save hard link without a target Card which has a valid Id.");

            if (link.Origin?.Id == null)
                throw new InvalidOperationException("Cannot save hard link without an origin Card which has a valid Id.");
        }

        protected override void AddParametersToInsertUpdateCommand(SQLiteCommand cmd, HardLink link)
        {
            cmd.Parameters.AddWithValue("@originId", link.Origin?.Id);
            cmd.Parameters.AddWithValue("targetId", link.Target?.Id);
        }

        protected override HardLink Parse(IDataRecord record)
        {
            var originID = record.ToInt("OriginId");
            var targetId = record.ToInt("TargetId");
            var hardlink = new HardLinkWrapper(CardDataAccess, originID, targetId);
            hardlink.Mutual = record.ToBool("Mutual");
            return hardlink;
        }
    }
}