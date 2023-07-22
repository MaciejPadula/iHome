import type { WidgetModel } from "../../models/widget";
import { httpget } from "./call-api-service";

export function getRoomWidgets(roomId: string): Promise<WidgetModel[]> {
  return httpget(`Widget/GetWidgets/${roomId}`);
}