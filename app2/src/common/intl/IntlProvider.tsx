import { type PropsWithChildren } from "react";
import { IntlProvider as IntlProviderRaw } from "react-intl";

import messages from "./locales/fr.json";

export const DEFAULT_LOCALE = "fr";

export const IntlProvider = (props: PropsWithChildren): JSX.Element => {
  return (
    <IntlProviderRaw locale={DEFAULT_LOCALE} messages={messages} {...props} />
  );
};
