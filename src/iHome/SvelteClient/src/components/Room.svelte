<script lang="ts">
  import { Button, Chevron, Dropdown, DropdownItem } from "flowbite-svelte";
    import type { RoomModel } from "../models/room";

  export let room: RoomModel;
  export let urlGenerator: Function;
</script>

<div class="p-4 w-full flex flex-row justify-between text-black dark:text-white">
  <a class="flex flex-row items-center w-full" href={urlGenerator(room.id)}>
    <div class="w-14 h-14 m-4">
      <img class="rounded-full w-full h-full" src="https://www.thespruce.com/thmb/iMt63n8NGCojUETr6-T8oj-5-ns=/1500x0/filters:no_upscale():max_bytes(150000):strip_icc()/PAinteriors-7-cafe9c2bd6be4823b9345e591e4f367f.jpg" alt="test">
    </div>
    <div class="flex flex-col">
      <p>{room.name}</p>
      <small>{room.user.name ?? room.user.email}</small>
    </div>
  </a>
  
  <Button id="room-{room.id}-menu">
    <svg class="w-6 h-6 text-gray-800 dark:text-white" aria-hidden="true" xmlns="http://www.w3.org/2000/svg" fill="currentColor" viewBox="0 0 20 20">
    <path d="M1 5h1.424a3.228 3.228 0 0 0 6.152 0H19a1 1 0 1 0 0-2H8.576a3.228 3.228 0 0 0-6.152 0H1a1 1 0 1 0 0 2Zm18 4h-1.424a3.228 3.228 0 0 0-6.152 0H1a1 1 0 1 0 0 2h10.424a3.228 3.228 0 0 0 6.152 0H19a1 1 0 0 0 0-2Zm0 6H8.576a3.228 3.228 0 0 0-6.152 0H1a1 1 0 0 0 0 2h1.424a3.228 3.228 0 0 0 6.152 0H19a1 1 0 0 0 0-2Z"/>
  </svg>
  </Button>
  <Dropdown triggeredBy="#room-{room.id}-menu">
    <DropdownItem>Share</DropdownItem>
    <DropdownItem>Remove</DropdownItem>
  </Dropdown>
</div>