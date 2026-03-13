import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { AxiosProvider } from "./context/axiosContext";
import type { PropsWithChildren } from "react";

const queryClient = new QueryClient();

const Providers = ({ children }: PropsWithChildren) => (
    <QueryClientProvider client={queryClient}>
        <AxiosProvider>
            {children}
        </AxiosProvider>
    </QueryClientProvider>
);

export default Providers
