import {BaseDto} from "./BaseDto";

export class ServerAddsClientToRoom extends BaseDto<ServerAddsClientToRoom> {
  lastMessages?: Message[];
  roomId?: number;
}

export class Message extends BaseDto<Message> {
  message?: string;
  timeStamp?: string;
  username?: string;
  roomId?: number;
}

