using Domain.Exceptions;
using Domain.Interfaces.Repositories.Specifics;
using Domain.Model;
using Domain.Static;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace DAL.Repositories.Specifics;

public class AdoExemplarStageProfileWriteRepository : IAdoExemplarStageProfileWriteRepository {
	private string _connstring { get; set; }

    // todo check value
	public AdoExemplarStageProfileWriteRepository(IConfiguration config) {
        _connstring = config[ApiConfig.ConnectionStrings_Main];
	}

    // todo manual + unit tests
	public StageProfile AddStageProfile(StageProfile stageProfile) {
        var StageProfileId = -1;

        string sql = "INSERT INTO StageProfile (FullName, OwnerId, About, WebsiteURL, Deleted) " +
                     "VALUES (@fullname, @ownerid, @about, @websiteurl, @deleted); " +
                     "SELECT CAST(scope_identity() AS int)";

        using (SqlConnection conn = new(_connstring)) {
            SqlCommand cmd = new(sql, conn);

            cmd.Parameters.Add("@fullname", SqlDbType.VarChar);
            cmd.Parameters.Add("@ownerid", SqlDbType.Int);
            cmd.Parameters.Add("@about", SqlDbType.NVarChar);
            cmd.Parameters.Add("@websiteurl", SqlDbType.NVarChar);
            cmd.Parameters.Add("@deleted", SqlDbType.Bit);

            cmd.Parameters["@fullname"].Value = stageProfile.FullName;
            cmd.Parameters["@ownerid"].Value = stageProfile.OwnerId;
            cmd.Parameters["@about"].Value = stageProfile.About;
            cmd.Parameters["@websiteurl"].Value = stageProfile.WebsiteURL;
            cmd.Parameters["@deleted"].Value = 0;

            try {
                conn.Open();
                StageProfileId = (int)cmd.ExecuteScalar();
            } catch (Exception exc) {
                throw new RepositoryException("Insert error", exc);
            }
        }

        stageProfile.Id = StageProfileId;

        return stageProfile.Id < 1 ? null : stageProfile;
    }
}
