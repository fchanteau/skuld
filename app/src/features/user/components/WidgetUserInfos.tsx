import { FormattedMessage } from 'react-intl';
import { Button } from 'reactstrap';

import { useCurrentUserQuery } from '../user.api';

export function WidgetUserInfos() {
  const { data } = useCurrentUserQuery();

  return (
    <div className="card">
      <div className="card-header bg-primary text-white fs-2">
        <FormattedMessage id="user.widget.title" />
      </div>
      <div className="card-body">
        <div className="row">
          <div className="col-12 col-md-8">
            <ul>
              <li className="fs-4">
                <FormattedMessage id="user.lastname" values={{ value: data?.lastName.toLocaleUpperCase() }} />
              </li>
              <li className="fs-4">
                <FormattedMessage id="user.firstname" values={{ value: data?.firstName }} />
              </li>
              <li className="fs-4">
                <FormattedMessage id="user.email" values={{ value: data?.email }} />
              </li>
            </ul>
          </div>
          <div className="col-4 d-none d-md-flex justify-content-center align-items-center">
            <h1>
              <i className="pe-2 bi bi-person-badge"></i>
            </h1>
          </div>
        </div>
      </div>
      <div className="card-footer bg-white d-flex justify-content-end">
        <Button outline color="primary">
          <FormattedMessage id="user.widget.edit" />
        </Button>
      </div>
    </div>
  );
}
