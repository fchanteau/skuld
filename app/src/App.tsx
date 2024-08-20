import { StrictMode } from 'react';
import { Provider } from 'react-redux';
import { RouterProvider } from 'react-router-dom';
import { ChakraProvider } from '@chakra-ui/react';

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
          <ChakraProvider>
            <MainContainer />
          </ChakraProvider>
        </StrictMode>
      </IntlProvider>
    </Provider>
  );
};

export default App;
