<script lang="ts">
  import { Button, ButtonGroup, Input } from "flowbite-svelte";
  import { user } from "../data-access/store";

  let name = '';

  export let remove: (e: MouseEvent) => void;
  export let save: (name: string) => void;

  function saveRoom(e: MouseEvent) {
    save(name);
  }

  $: username = $user?.name ?? $user?.email;
</script>

<div class="p-4 w-full flex flex-row justify-between text-black dark:text-white">
  <div class="flex flex-row items-center w-full">
    <div class="w-14 h-14 m-4">
      <img class="rounded-full w-full h-full" src="https://www.thespruce.com/thmb/iMt63n8NGCojUETr6-T8oj-5-ns=/1500x0/filters:no_upscale():max_bytes(150000):strip_icc()/PAinteriors-7-cafe9c2bd6be4823b9345e591e4f367f.jpg" alt="test">
    </div>
    <div class="flex flex-col">
      <Input bind:value={name} type="text" placeholder="New Room Name" required  />
      <small>{username}</small>
    </div>
  </div>
  <div>
    <div class="flex flex-col sm:flex-row">
      <ButtonGroup>
        <Button on:click={remove} color="alternative">
          <svg class="w-6 h-6 text-gray-800 dark:text-white" aria-hidden="true" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 14 14">
            <path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="m1 1 6 6m0 0 6 6M7 7l6-6M7 7l-6 6"/>
          </svg>
        </Button>
        <Button on:click={saveRoom} color="primary">
          <svg class="w-6 h-6 text-gray-800 dark:text-white" aria-hidden="true" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 16 12">
            <path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M1 5.917 5.724 10.5 15 1.5"/>
          </svg>
        </Button>
      </ButtonGroup>
    </div>
    
  </div>
</div>