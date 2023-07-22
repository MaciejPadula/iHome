import axios, { type RawAxiosRequestHeaders } from "axios";
import auth from "../auth-service";

const baseUrl = 'https://localhost:32678/api';

async function getHeaders(authenticated: boolean): Promise<RawAxiosRequestHeaders> {
  if (!authenticated) {
    return {};
  }

  const token = await auth.getToken();

  return {
    Authorization: `Bearer ${await auth.getToken()}`
  };
}

export async function httpget<T>(endpoint: string, authenticated = true): Promise<T> {
  const response = await axios.get<T>(`${baseUrl}/${endpoint}`, {
      withCredentials: true,
      headers: await getHeaders(authenticated)
  });

  return response.data;
}

export async function httppost<T>(endpoint: string, body: object, authenticated = true): Promise<T> {
  const response = await axios.post<T>(`${baseUrl}/${endpoint}`, body, {
      withCredentials: true,
      headers: await getHeaders(authenticated)
  });

  return response.data;
}