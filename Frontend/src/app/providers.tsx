import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { AxiosProvider } from "./context/axiosContext";
import type { PropsWithChildren } from "react";
import axios from "axios";

const queryClient = new QueryClient({
    defaultOptions: {
        queries: {
            retry: (failureCount, error) => {
                // Retry 3 times on non 404 errors
                if (axios.isAxiosError(error)) {
                    if (error.response?.status === 404) {
                        return false;
                    }
                    if (error.code === 'ECONNABORTED') {
                        return failureCount < 1;
                    }
                }
                return failureCount < 3;
            },
            retryDelay: (attempt) => Math.min(1000 * 2 ** attempt, 5000), // exponential backoff
        },
        mutations: {
            retry: 1, // Retry 1 time on mutations
        },
    },
});

const Providers = ({ children }: PropsWithChildren) => (
    <QueryClientProvider client={queryClient}>
        <AxiosProvider>
            {children}
        </AxiosProvider>
    </QueryClientProvider>
);

export default Providers
