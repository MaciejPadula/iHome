import axios from "axios";
import auth from "../auth-service";

const baseUrl = 'https://localhost:32678/api';

export async function get<T>(endpoint: string): Promise<T> {
  const client = await auth.createClient();
  const token = await client.getTokenSilently();

  const response = await axios.get<T>(`${baseUrl}/${endpoint}`, {
      withCredentials: true,
      headers: {
          Authorization: `Bearer ${token}`
      }
  });

  return response.data;
}

export async function post<T>(endpoint: string, body: object): Promise<T> {
  const client = await auth.createClient();
  const token = await client.getTokenSilently();

  const response = await axios.post<T>(`${baseUrl}/${endpoint}`, body, {
      withCredentials: true,
      headers: {
          Authorization: `Bearer ${token}`
      }
  });

  return response.data;
}