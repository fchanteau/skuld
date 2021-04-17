using AutoMapper;
using Skuld.Data.UnitOfWork;

namespace Skuld.Shared.Services
{
    public abstract class BaseService
    {
        #region Private properties

        protected readonly UnitOfWork _unitOfWork;
        protected IMapper _mapper;

        #endregion

        #region Constructor

        public BaseService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion
    }
}
