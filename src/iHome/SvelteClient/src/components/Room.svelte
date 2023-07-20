<script lang="ts">
  import { Button } from "flowbite-svelte";
  import { Card } from "flowbite-svelte";
  import type { Room } from "../models/room";

  export let room: Room;
  export let urlGenerator: VoidFunction<string>;
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
  
  <Button class="!p-2">
    <svg aria-hidden="true" class="w-5 h-5" fill="currentColor" viewBox="0 0 20 20" xmlns="http://www.w3.org/2000/svg">
      <path fill-rule="evenodd" d="M10.293 3.293a1 1 0 011.414 0l6 6a1 1 0 010 1.414l-6 6a1 1 0 01-1.414-1.414L14.586 11H3a1 1 0 110-2h11.586l-4.293-4.293a1 1 0 010-1.414z" clip-rule="evenodd"></path>
    </svg>
  </Button>
</div>