import {
  Container,
  Stack,
  Heading,
  Box,
  FormControl,
  FormLabel,
  Input,
  HStack,
  Checkbox,
  Button,
  Divider,
  Text,
} from "@chakra-ui/react";
import { FormattedMessage } from "react-intl";

import { PasswordField } from "./PasswordField";

export function Login(): JSX.Element {
  return (
    <Container
      maxW="lg"
      py={{ base: "12", md: "24" }}
      px={{ base: "0", sm: "8" }}
    >
      <Stack spacing="8">
        <Stack spacing="6">
          <Stack spacing={{ base: "2", md: "3" }} textAlign="center">
            <Heading size={{ base: "xs", md: "sm" }}>
              <FormattedMessage id="auth.login" />
            </Heading>
            <Text color="fg.muted">
              <FormattedMessage id="auth.register.phrase" />
            </Text>
          </Stack>
        </Stack>
        <Box
          py={{ base: "0", sm: "8" }}
          px={{ base: "4", sm: "10" }}
          bg={{ base: "transparent", sm: "bg.surface" }}
          boxShadow={{ base: "none", sm: "md" }}
          borderRadius={{ base: "none", sm: "xl" }}
        >
          <Stack spacing="6">
            <Stack spacing="5">
              <FormControl>
                <FormLabel htmlFor="email">Email</FormLabel>
                <Input id="email" type="email" />
              </FormControl>
              <PasswordField />
            </Stack>
            <HStack justify="space-between">
              <Checkbox defaultChecked>Remember me</Checkbox>
              <Button variant="text" size="sm">
                Forgot password?
              </Button>
            </HStack>
            <Stack spacing="6">
              <Button colorScheme="teal">Sign in</Button>
              <HStack>
                <Divider />
                <Text textStyle="sm" whiteSpace="nowrap" color="fg.muted">
                  or continue with
                </Text>
                <Divider />
              </HStack>
            </Stack>
          </Stack>
        </Box>
      </Stack>
    </Container>
  );
}
