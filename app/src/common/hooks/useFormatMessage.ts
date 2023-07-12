import { useCallback } from 'react';
import { useIntl } from 'react-intl';

import { type IntlMessages } from 'typings/intl';

export type FormatMessage = (messageIdentifier: IntlMessages, defaultMessage?: string) => string;

export const useFormatMessage = (): FormatMessage => {
  const { formatMessage } = useIntl();

  return useCallback(
    (messageIdentifier, defaultMessage) => {
      return formatMessage({ id: messageIdentifier, defaultMessage });
    },
    [formatMessage],
  );
};
