import { Component } from '@angular/core';
import {FormControl} from "@angular/forms";
import {BaseDto, ClientWantsToBroadCastToRoom, ClientWantsToEnterRoom, ClientWantsToSignIn} from "../BaseDto";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'Chatrooms';
  messages: string[] = [];

  ws: WebSocket = new WebSocket('ws://localhost:8181');
  messageContent = new FormControl('');
  SelectedUserName = new FormControl('');
  SelectedRoomNumber  = new FormControl('');
  SentMessage= new FormControl('');
 constructor() {
   this.ws.onmessage = (event) => {
     this.messages.push(event.data);
   }
 }


  sendMessage() {
    let o = new ClientWantsToBroadCastToRoom({Message: this.messageContent.value?.toString()})
    this.ws.send(JSON.stringify(o));
    
  }

  signInUser() {
   let o = new ClientWantsToSignIn({Username: this.SelectedUserName.value?.toString()})
      this.ws.send(JSON.stringify(o));
  }

  selectRoom() {
   var num = parseInt(this.SelectedRoomNumber.value!);
   let x = new ClientWantsToEnterRoom({RoomId: num })
    this.ws.send(JSON.stringify(x));
  }

  protected readonly BaseDto = BaseDto;


}
