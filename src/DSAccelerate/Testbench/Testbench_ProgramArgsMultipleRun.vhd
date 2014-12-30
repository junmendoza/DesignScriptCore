--------------------------------------------------------------------------------
-- Company: 
-- Engineer:
--
-- Create Date:   18:26:37 12/17/2014
-- Design Name:   
-- Module Name:   D:/jun/Research/git/DSAccelerate/Testbench/Testbench_ProgramArgsMultipleRun.vhd
-- Project Name:  SynthesizedProgram
-- Target Device:  
-- Tool versions:  
-- Description:   
-- 
-- VHDL Test Bench Created by ISE for module: ProgramArgs
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
 
ENTITY Testbench_ProgramArgsMultipleRun IS
END Testbench_ProgramArgsMultipleRun;
 
ARCHITECTURE behavior OF Testbench_ProgramArgsMultipleRun IS 
 
   -- Component Declaration for the Unit Under Test (UUT)
 
    COMPONENT ProgramArgs
    PORT(
			clock 	: in STD_LOGIC;
			reset	 	: in STD_LOGIC;
			sw1	 	: in STD_LOGIC;
			sw2	 	: in STD_LOGIC;
			sw3	 	: in STD_LOGIC;
			execute 	: in STD_LOGIC;
			a_out 	: out STD_LOGIC_VECTOR (31 downto 0);
			b_out 	: out STD_LOGIC_VECTOR (31 downto 0);
			c_out 	: out STD_LOGIC_VECTOR (31 downto 0);
			LCD_E 	: out STD_LOGIC;
			LCD_RS 	: out STD_LOGIC;
			LCD_RW	: out STD_LOGIC;
			LCD_DB	: out STD_LOGIC_VECTOR(7 downto 0);
			LED 		: out STD_LOGIC_VECTOR(7 downto 0)
        );
    END COMPONENT;
    

   --Inputs
   signal clock : std_logic := '0';
   signal reset : std_logic := '1';
   signal sw1 : std_logic := '0';
   signal sw2 : std_logic := '0';
   signal sw3 : std_logic := '0';
   signal execute : std_logic := '0';

 	--Outputs
   signal a_out 	: std_logic_vector(31 downto 0);
   signal b_out 	: std_logic_vector(31 downto 0);
   signal c_out 	: std_logic_vector(31 downto 0);
	signal LCD_E 	: STD_LOGIC;
	signal LCD_RS 	: STD_LOGIC;
	signal LCD_RW	: STD_LOGIC;
	signal LCD_DB	: STD_LOGIC_VECTOR(7 downto 0);
	signal LED 		: STD_LOGIC_VECTOR(7 downto 0);

   -- Clock period definitions
	constant clkCycles : integer := 1000;
 
BEGIN
 
	-- Instantiate the Unit Under Test (UUT)
   uut: ProgramArgs PORT MAP (
          clock 	=> clock,
          reset 	=> reset,
          sw1		=> sw1,
          sw2 		=> sw2,
			 sw3		=> sw3,
			 execute	=> execute,
          a_out 	=> a_out,
          b_out 	=> b_out,
          c_out 	=> c_out,
			 LCD_E 	=> LCD_E, 	
			 LCD_RS 	=> LCD_RS, 	
			 LCD_RW	=> LCD_RW,	
			 LCD_DB	=> LCD_DB,	
			 LED 		=> LED 	
        );


	-- Stimulus process
   stim_proc: process
   begin		
		
		-----------------------------------
		-- Initialize and reset
		-----------------------------------
		clock <= '0';
		reset <= '1';
		wait for 5 ns;
		
		-- Hold
		for a in 1 to clkCycles loop
			clock <= not clock;
			wait for 5 ns;
		end loop;
		
		-- Set input
		clock <= '1';
		reset <= '0';
		sw3 <= '0';
		sw2 <= '0';
		sw1 <= '0';
		wait for 5 ns;
		
		clock <= '0';
		wait for 5 ns;
		
		-----------------------------------
		-- Sequence 1 
		-- Execute with args sw3-sw1 = 000 
		-----------------------------------
		execute <= '1';
		clock <= '1';
		wait for 5 ns;
		
		clock <= '0';
		wait for 5 ns;
		
		-- Reset execute flag
		clock <= '1';
		execute <= '0';
		wait for 5 ns;
		
		-- Hold
		for b in 1 to clkCycles loop
			clock <= not clock;
			wait for 5 ns;
		end loop;
		
		clock <= '0';
		wait for 5 ns;
		
		-- Set next preview
		clock <= '1';
		sw3 <= '0';
		sw2 <= '0';
		sw1 <= '1';
		wait for 5 ns;
		
		-- Hold
		for c in 1 to clkCycles loop
			clock <= not clock;
			wait for 5 ns;
		end loop;
		
		
		-----------------------------------
		-- Initialize and reset
		-----------------------------------
		clock <= '0';
		reset <= '1';
		wait for 5 ns;
		
		-- Hold
		for d in 1 to clkCycles loop
			clock <= not clock;
			wait for 5 ns;
		end loop;
		
		-- Set input
		clock <= '1';
		reset <= '0';
		sw3 <= '0';
		sw2 <= '1';
		sw1 <= '0';
		wait for 5 ns;
		
		clock <= '0';
		wait for 5 ns;
		
		-----------------------------------
		-- Sequence 2
		-- Execute with args sw3-sw1 = 001 
		-----------------------------------
		execute <= '1';
		clock <= '1';
		wait for 5 ns;
		
		clock <= '0';
		wait for 5 ns;
		
		-- Reset execute flag
		clock <= '1';
		execute <= '0';
		wait for 5 ns;
		
		-- Hold
		for e in 1 to clkCycles loop
			clock <= not clock;
			wait for 5 ns;
		end loop;
			
		clock <= '0';
		wait for 5 ns;
		
		-- Set next preview
		clock <= '1';
		sw3 <= '0';
		sw2 <= '0';
		sw1 <= '1';
		wait for 5 ns;
		
		-- Hold
		for f in 1 to clkCycles loop
			clock <= not clock;
			wait for 5 ns;
		end loop;
		
      wait;
		
   end process; 

END;
