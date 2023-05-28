using AutoMapper;
using Skuld.Data.UnitOfWork;

namespace Skuld.WebApi.Services
{
	public abstract class BaseService
	{
		#region Private properties

		protected readonly IUnitOfWork UnitOfWork;
		public IMapper Mapper;

		#endregion

		#region Constructor

		public BaseService (IUnitOfWork unitOfWork)
		{
			UnitOfWork = unitOfWork;
		}

		#endregion
	}
}
