/* eslint-disable no-unused-vars */
import { useCallback } from "react";
import { type PrimitiveType, useIntl } from "react-intl";

import { IntlMessages } from "./intl";

export type FormatMessage = (
  messageIdentifier: IntlMessages,
  values?: Record<string, PrimitiveType>,
  defaultMessage?: string
) => string;

export const useFormatMessage = (): FormatMessage => {
  const { formatMessage } = useIntl();

  return useCallback(
    (messageIdentifier, values, defaultMessage) => {
      return formatMessage({ id: messageIdentifier, defaultMessage }, values);
    },
    [formatMessage]
  );
};
