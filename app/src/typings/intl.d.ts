import type messages from '../common/intl/locales/fr.json';

export type IntlMessages = keyof typeof messages;

declare global {
  namespace FormatjsIntl {
    interface Message {
      ids: IntlMessages;
    }
    interface IntlConfig {
      locale: 'fr';
    }
  }
}
