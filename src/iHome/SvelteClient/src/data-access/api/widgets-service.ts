import type { WidgetModel } from "../../models/widget";
import { get } from "./call-api-service";

export function getRoomWidgets(roomId: string): Promise<WidgetModel[]> {
  return get(`Widget/GetWidgets/${roomId}`);
}