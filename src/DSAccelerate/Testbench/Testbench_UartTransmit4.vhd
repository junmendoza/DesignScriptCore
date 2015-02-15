--------------------------------------------------------------------------------
-- Company: 
-- Engineer:
--
-- Create Date:   13:18:15 02/15/2015
-- Design Name:   
-- Module Name:   D:/jun/Research/git/DesignScriptCore/src/DSAccelerate/Testbench/Testbench_UartTransmit4.vhd
-- Project Name:  SynthesizedProgram
-- Target Device:  
-- Tool versions:  
-- Description:   
-- 
-- VHDL Test Bench Created by ISE for module: UartTransmit4
-- 
-- Dependencies:
-- 
-- Revision:
-- Revision 0.01 - File Created
-- Additional Comments:
--
-- Notes: 
-- This testbench has been automatically generated using types std_logic and
-- std_logic_vector for the ports of the unit under test.  Xilinx recommends
-- that these types always be used for the top-level I/O of a design in order
-- to guarantee that the testbench will bind correctly to the post-implementation 
-- simulation model.
--------------------------------------------------------------------------------
LIBRARY ieee;
USE ieee.std_logic_1164.ALL;
 
-- Uncomment the following library declaration if using
-- arithmetic functions with Signed or Unsigned values
--USE ieee.numeric_std.ALL;
 
ENTITY Testbench_UartTransmit4 IS
END Testbench_UartTransmit4;
 
ARCHITECTURE behavior OF Testbench_UartTransmit4 IS 
 
    -- Component Declaration for the Unit Under Test (UUT)
 
    COMPONENT UartTransmit4
    PORT(
         clock : IN  std_logic;
         reset : IN  std_logic;
         start_transmit_4bytes : IN  std_logic;
         data_4bytes : IN  std_logic_vector(31 downto 0);
         send_4bytes_complete : OUT  std_logic;
         RS232_dataout : OUT  std_logic
        );
    END COMPONENT;
    

   --Inputs
   signal clock : std_logic := '0';
   signal reset : std_logic := '1';
   signal start_transmit_4bytes : std_logic := '0';
   signal data_4bytes : std_logic_vector(31 downto 0) := (others => '0');

 	--Outputs
   signal send_4bytes_complete : std_logic;
   signal RS232_dataout : std_logic;

BEGIN
 
	-- Instantiate the Unit Under Test (UUT)
   uut: UartTransmit4 PORT MAP (
          clock => clock,
          reset => reset,
          start_transmit_4bytes => start_transmit_4bytes,
          data_4bytes => data_4bytes,
          send_4bytes_complete => send_4bytes_complete,
          RS232_dataout => RS232_dataout
        );

	-- Stimulus process
   stim_proc: process
   begin		
	
		reset <= '1';
		wait for 5 ns;		
		
		-- Begin execution
		reset <= '0';
		start_transmit_4bytes <= '1';
		data_4bytes <= X"01020304";
		wait for 5 ns;
		
		for a in 1 to 1000000 loop
			clock <= not clock;
			StopTransmitterIfSendComplete : if send_4bytes_complete = '1' then
				start_transmit_4bytes <= '0';
			end if StopTransmitterIfSendComplete;
			wait for 5 ns;
		end loop;
		
      wait;
   end process;
END;
