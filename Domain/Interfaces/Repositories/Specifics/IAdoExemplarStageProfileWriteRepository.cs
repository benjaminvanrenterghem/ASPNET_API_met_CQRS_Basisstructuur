using Domain.Model;

namespace Domain.Interfaces.Repositories.Specifics;

public interface IAdoExemplarStageProfileWriteRepository {
	StageProfile AddStageProfile(StageProfile stageProfile);
}
