import { loading } from "./store";

export async function executeWithLoadingAsync(func: Function) {
  loading.set(true);
  await func();
  loading.set(false);
}