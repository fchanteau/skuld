import { StrictMode } from 'react';
import { Provider } from 'react-redux';
import { RouterProvider } from 'react-router-dom';

import { IntlProvider } from './common/intl';
import { router } from './pages/router';
import { store } from './bootstrap';

const MainContainer = (): JSX.Element => {
  return <RouterProvider router={router} />;
};

const App = (): JSX.Element => {
  return (
    <Provider store={store}>
      <IntlProvider>
        <StrictMode>
          <MainContainer />
        </StrictMode>
      </IntlProvider>
    </Provider>
  );
};

export default App;
