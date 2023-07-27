import type { WidgetModel } from "../../models/widget";
import { httpget } from "./call-api-service";

const prefix = "Widget/";

export function getRoomWidgets(roomId: string): Promise<WidgetModel[]> {
  return httpget(`${prefix}GetWidgets/${roomId}`);
}