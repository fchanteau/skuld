import { ChakraProvider } from "@chakra-ui/react";
import { StrictMode } from "react";
import { Provider } from "react-redux";

import { IntlProvider } from "./common/intl/IntlProvider";
import { store } from "./common/store/store";
import { AuthPage } from "./pages/AuthPage";

function App(): JSX.Element {
  return (
    <StrictMode>
      <IntlProvider>
        <Provider store={store}>
          <ChakraProvider>
            <AuthPage />
          </ChakraProvider>
        </Provider>
      </IntlProvider>
    </StrictMode>
  );
}

export default App;
