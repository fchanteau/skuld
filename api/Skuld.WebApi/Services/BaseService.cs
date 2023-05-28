using AutoMapper;
using Skuld.Data.UnitOfWork;

namespace Skuld.WebApi.Services
{
	public abstract class BaseService
	{
		#region Private properties

		protected readonly UnitOfWork _unitOfWork;
		public IMapper Mapper;

		#endregion

		#region Constructor

		public BaseService (UnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		#endregion
	}
}
