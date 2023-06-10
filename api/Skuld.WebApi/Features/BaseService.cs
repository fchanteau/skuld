using Skuld.Data.UnitOfWork;

namespace Skuld.WebApi.Features
{
	public abstract class BaseService
	{
		#region Private properties

		protected readonly IUnitOfWork UnitOfWork;

		#endregion

		#region Constructor

		public BaseService (IUnitOfWork unitOfWork)
		{
			UnitOfWork = unitOfWork;
		}

		#endregion
	}
}
